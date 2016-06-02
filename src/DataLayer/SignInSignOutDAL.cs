using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using V2.Orbit.Model;
using V2.CommonServices.Exceptions;
using V2.CommonServices.FileLogger;


namespace V2.Orbit.DataLayer
{
    [Serializable]
    public class SignInSignOutDAL : DBBaseClass
    {
        DataSet dsSignInSignOutDAL = new DataSet();
        DataSet dsWeeklyOffDAL = new DataSet();

        #region weekly off for Shift Employee
        public DataSet GetWeeklyOff(SignInSignOutModel objWeeklyOffModel)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
            param[0].Value = objWeeklyOffModel.EmployeeID;
            DateTime nullDate = new DateTime();
            if (objWeeklyOffModel.FromDate== nullDate)
                param[1] = new SqlParameter("@StartDate", DBNull.Value);
            else
                param[1] = new SqlParameter("@StartDate", objWeeklyOffModel.FromDate);

            if (objWeeklyOffModel.Todate == nullDate)
                param[2] = new SqlParameter("@EndDate", DBNull.Value);
            else
                param[2] = new SqlParameter("@EndDate", objWeeklyOffModel.Todate);   

            

            dsWeeklyOffDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetWeekOffDate", param);
            return dsWeeklyOffDAL;
        }
        #endregion

        #region Binding data to Grid

        public DataSet BindSignInSignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.EmployeeID;
                //Commented by viveka 'April/09/2008'
                param[1] = new SqlParameter("@ColumnName", SqlDbType.NVarChar);
                param[1].Value = objSignInSignOutModel.ColumnName;
                param[2] = new SqlParameter("@SortOrder", SqlDbType.NVarChar);
                param[2].Value = objSignInSignOutModel.SortOrder;


                dsSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "BindDataForSignInSignOut1", param);
                for (int i = 0; i < dsSignInSignOutDAL.Tables[0].Rows.Count; i++)
                {
                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["IsBulk"].ToString() == "True")
                    {
                        dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Bulk";
                    }
                    else
                    {
                        if (dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"].ToString() == "1")
                        {
                            dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Manual";
                        }
                        else if (dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"].ToString() == "0")
                        {
                            dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Auto";
                        }
                    }
                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString() != "")
                    {
                        string str = dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString();
                        char[] chrSplit = { ':' };
                        string[] strArray = new string[2];
                        strArray = str.Split(chrSplit);
                        if (strArray[0] == "##Leave")
                        {
                            str = str.Replace(strArray[0], "");
                            str = str.Replace(":", "");
                            dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"] = str;
                        }

                    }
                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["SignInTime"].ToString() != "")
                    {

                        string str = dsSignInSignOutDAL.Tables[0].Rows[i]["SignInTime"].ToString();
                        str = str.Substring(0, 5);
                        dsSignInSignOutDAL.Tables[0].Rows[i]["SignInTime"] = str;

                    }

                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["SignOutTime"].ToString() != "")
                    {

                        string str = dsSignInSignOutDAL.Tables[0].Rows[i]["SignOutTime"].ToString();
                        str = str.Substring(0, 5);
                        dsSignInSignOutDAL.Tables[0].Rows[i]["SignOutTime"] = str;

                    }

                    if ((dsSignInSignOutDAL.Tables[0].Rows[i]["EmployeeName"].ToString() == "") && (dsSignInSignOutDAL.Tables[0].Rows[i]["Status"].ToString() == "Approved"))
                    {
                        dsSignInSignOutDAL.Tables[0].Rows[i]["Status"] = "Auto Approved";
                    }
                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"].ToString() != "")
                    {
                        char[] chrSplitter1 = { ':' };
                        string[] strArray1 = new string[2];
                        string str1 = dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"].ToString();
                        strArray1 = str1.Split(chrSplitter1);

                        //Added by Rahul Ramachandran to avoid array bound exception.
                        for (int count = 0; count < strArray1.Length; count++)
                        {
                            if(strArray1[count].Length==1)
                            {
                                strArray1[count] = "0" + strArray1[count];
                            }                           
                        }
                        if (strArray1.Length == 3)
                        {
                            str1 = strArray1[0] + ":" + strArray1[1]+":"+strArray1[2];
                        }
                        else if (strArray1.Length == 2)
                        {
                            str1 = strArray1[0] + ":" + strArray1[1];
                        }
                        else
                            str1 = strArray1[0];

                        //str1 = strArray1[0] + ":" + strArray1[1];
                        dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"] = str1;

                        //    if (strArray1[0].Length == 1)
                        //    {
                        //        strArray1[0] = "0" + strArray1[0];
                        //    }
                        //if (strArray1[1].Length == 1)
                        //{
                        //    strArray1[1] = "0" + strArray1[1];
                        //}                       

                    }

                }
                return dsSignInSignOutDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "BindSignInSignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        #region Auto Sign-In

        //check if the employee is on leave -- Employee will not be allowed to sign-in if he has an approved leave 
        public DataSet CheckLeaveDetails(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.EmployeeID;

                param[1] = new SqlParameter("@Signin", SqlDbType.DateTime);
                param[1].Value = objSignInSignOutModel.SignInTime;

                dsSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "CheckLeaveDetailsForSignIn", param);
                return dsSignInSignOutDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "CheckLeaveDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        //check for the missing Sign out entries. Employee will not be allowed to sign in unless he has singed out for the corr sign-ins.
        public DataSet CheckMissingSignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.EmployeeID;

                dsSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "CheckMissingSignOut", param);
                return dsSignInSignOutDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "CheckMissingSignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        // Check if the Employee has already Signed_in for the day
        public DataSet CheckForMultipleSignIn(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.EmployeeID;

                param[1] = new SqlParameter("@Signin", SqlDbType.DateTime);
                param[1].Value = objSignInSignOutModel.SignInTime;

                dsSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "CheckForMultipleSignIns", param);
                return dsSignInSignOutDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "CheckForMultipleSignIn", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }


        // after the above 3 functions(conditions) are satisfied(return false) the employee is allowed to sign-in.
        public SqlDataReader SignIn(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.EmployeeID;
                param[1] = new SqlParameter("@IsSignInManual", SqlDbType.Bit);
                param[1].Value = objSignInSignOutModel.IsSignInManual;
                param[2] = new SqlParameter("@IsSignOutManual", SqlDbType.Bit);
                param[2].Value = objSignInSignOutModel.IsSignOutManual;
                param[3] = new SqlParameter("@signin", SqlDbType.DateTime);
                param[3].Value = objSignInSignOutModel.SignInTime;


                SqlDataReader sdrSignIn = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "SignIn", param);
                return sdrSignIn;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "SignIn", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        #endregion

        #region SignOut
        //Check if the Employee has already signed out for the day

        /*Commented by rahul :
         Reason: This was working for the old SP.The SP has been changed.*/
        public string MultipleSignOuts(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.EmployeeID;

                SqlDataReader sdrMultipleSignOuts = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "MultipleSignOuts", param);
                string SignInSignOutID = "";
                if (sdrMultipleSignOuts.Read())
                {

                    SignInSignOutID = sdrMultipleSignOuts["SignInSignOutID"].ToString();
                }
                else
                {
                    SignInSignOutID = "";
                }
                return SignInSignOutID;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "MultipleSignOuts", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(), ex);
            }

        }

        //public string MultipleSignOuts(SignInSignOutModel objSignInSignOutModel)
        //{
        //    try
        //    {
        //        SqlParameter[] param = new SqlParameter[2];
        //        param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
        //        param[0].Value = objSignInSignOutModel.EmployeeID;
        //        param[1] = new SqlParameter("@SignoutTime", SqlDbType.DateTime);
        //        param[1].Value = objSignInSignOutModel.SignOutTime;
        //        SqlDataReader sdrMultipleSignOuts = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "MultipleSignOuts", param);
        //        string SignInSignOutID = "";
        //        if (sdrMultipleSignOuts.Read())
        //        {

        //            SignInSignOutID = sdrMultipleSignOuts["RESULT"].ToString();
        //        }
        //        else
        //        {
        //            SignInSignOutID = "";
        //        }
        //        return SignInSignOutID;
        //    }
        //    catch (V2Exceptions ex)
        //    {
        //        throw;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        FileLog objFileLog = FileLog.GetLogger();
        //        objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "MultipleSignOuts", ex.StackTrace);
        //        throw new V2Exceptions(ex.ToString(), ex);
        //    }

        //}

        //Auto Sign Out
        public SqlDataReader SignOut(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@SignInSignOutID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.SignInSignOutID;
                param[1] = new SqlParameter("@Signout", SqlDbType.DateTime);
                param[1].Value = objSignInSignOutModel.SignOutTime;

                SqlDataReader sdrSignOut = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "SignOut", param);
                return sdrSignOut;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "SignOut", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }



        #endregion

        #region Search
        public DataSet Search(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                //DataSet dsGetAbsentData = new DataSet();
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AbsenteeismFilter";
                cmd.CommandTimeout = 6000;

                cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
                cmd.Parameters["@EmployeeID"].Value = objSignInSignOutModel.EmployeeID;

                cmd.Parameters.Add("@Type", SqlDbType.Int);
                cmd.Parameters["@Type"].Value = objSignInSignOutModel.StatusID;

                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime);
                cmd.Parameters["@ToDate"].Value = objSignInSignOutModel.Todate;

                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime);
                cmd.Parameters["@FromDate"].Value = objSignInSignOutModel.FromDate;

                cmd.Parameters.Add("@ColumnName", SqlDbType.NVarChar);
                cmd.Parameters["@ColumnName"].Value = objSignInSignOutModel.ColumnName;

                cmd.Parameters.Add("@SortOrder", SqlDbType.NVarChar);
                cmd.Parameters["@SortOrder"].Value = objSignInSignOutModel.SortOrder;

                cmd.Connection = con;

                SqlDataAdapter da;

                if (objSignInSignOutModel.StatusID == 2)
                {
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dsSignInSignOutDAL);
                    DataTable dt = new DataTable();
                    for (int i = 0; i < dsSignInSignOutDAL.Tables[0].Rows.Count; i++)
                    {
                        if (dsSignInSignOutDAL.Tables[0].Rows[i]["AbsentDates"].ToString() != "")
                        {
                            string strDates = dsSignInSignOutDAL.Tables[0].Rows[i]["AbsentDates"].ToString();
                            string[] strDatesArray = new string[600];
                            char[] chrSplit = { ',' };
                            strDatesArray = strDates.Split(chrSplit);

                            dt.Columns.Add("SignInSignOutID");
                            dt.Columns.Add("date");
                            dt.Columns.Add("SignInTime");
                            dt.Columns.Add("SignOutTime");
                            dt.Columns.Add("TotalHoursWorked");
                            dt.Columns.Add("Mode");
                            dt.Columns.Add("SignInComment");
                            dt.Columns.Add("SignOutComment");
                            dt.Columns.Add("Status");
                            dt.Columns.Add("EmployeeName");
                            dt.Columns.Add("ApproverComments");
                            for (int j = 1; j < strDatesArray.Length; j++)
                            {
                                DataRow dr = dt.NewRow();
                                dr["SignInSignOutID"] = "";
                                dr["date"] = strDatesArray[j].ToString();
                                dr["SignInTime"] = "00:00";
                                dr["SignOutTime"] = "00:00";
                                dr["TotalHoursWorked"] = "00:00";
                                dr["Mode"] = "";
                                dr["SignInComment"] = "";
                                dr["SignOutComment"] = "";
                                dr["Status"] = "";
                                dr["EmployeeName"] = "";
                                dr["ApproverComments"] = "";
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    dsSignInSignOutDAL.Tables[0].Clear();
                    dsSignInSignOutDAL.Tables[0].Merge(dt);
                }
                else
                {
                    cmd.CommandText = "SignInSignOutSearch";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dsSignInSignOutDAL);

                    for (int i = 0; i < dsSignInSignOutDAL.Tables[0].Rows.Count; i++)
                    {
                        if (dsSignInSignOutDAL.Tables[0].Rows[i]["IsBulk"].ToString() == "True")
                        {
                            dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Bulk";
                        }
                        else
                        {
                            if (dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"].ToString() == "1")
                            {
                                dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Manual";
                            }
                            else if (dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"].ToString() == "0")
                            {
                                dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Auto";
                            }
                        }
                        if (dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString() != "")
                        {
                            string str = dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString();
                            char[] chrSplit = { ':' };
                            string[] strArray = new string[2];
                            strArray = str.Split(chrSplit);
                            if (strArray[0] == "##Leave")
                            {
                                str = str.Replace(strArray[0], "");
                                str = str.Replace(":", "");
                                dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"] = str;
                            }

                        }
                        if ((dsSignInSignOutDAL.Tables[0].Rows[i]["EmployeeName"].ToString() == "") && (dsSignInSignOutDAL.Tables[0].Rows[i]["Status"].ToString() == "Approved"))
                        {
                            dsSignInSignOutDAL.Tables[0].Rows[i]["Status"] = "Auto Approved";
                        }
                        if (dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"].ToString() != "")
                        {
                            char[] chrSplitter1 = { ':' };
                            string[] strArray1 = new string[2];
                            string str1 = dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"].ToString();
                            strArray1 = str1.Split(chrSplitter1);
                            if (strArray1[0].Length == 1)
                            {
                                strArray1[0] = "0" + strArray1[0];
                            }
                            if (strArray1[1].Length == 1)
                            {
                                strArray1[1] = "0" + strArray1[1];
                            }
                            str1 = strArray1[0] + ":" + strArray1[1];
                            dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"] = str1;

                        }

                    }
                }
                return dsSignInSignOutDAL;
            }

            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "Search", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }




        #endregion

        #region GetBulkEntries

        public DataSet GetBulkEntries(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.EmployeeID;

                dsSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetBulkEntries", param);

                return dsSignInSignOutDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "GetBulkEntries", ex.StackTrace);
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
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@EmployeeID", DbType.Int32);
                param[0].Value = objSignInSignOutModel.EmployeeID;
                param[1] = new SqlParameter("@Status", SqlDbType.Int);
                param[1].Value = objSignInSignOutModel.StatusID;
                param[2] = new SqlParameter("@ColumnName", SqlDbType.NVarChar);
                param[2].Value = objSignInSignOutModel.ColumnName;
                param[3] = new SqlParameter("@SortOrder", SqlDbType.NVarChar);
                param[3].Value = objSignInSignOutModel.SortOrder;
                dsSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "BindDataForSignInSignOutApproval", param);

                for (int i = 0; i < dsSignInSignOutDAL.Tables[0].Rows.Count; i++)
                {
                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["IsBulk"].ToString() == "True")
                    {
                        dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Bulk";
                    }
                    else
                    {
                        if (dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"].ToString() == "1")
                        {
                            dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Manual";
                        }
                        else if (dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"].ToString() == "0")
                        {
                            dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Auto";
                        }
                    }
                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString() != "")
                    {
                        string str = dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString();
                        char[] chrSplit = { ':' };
                        string[] strArray = new string[2];
                        strArray = str.Split(chrSplit);
                        if (strArray[0] == "##Leave")
                        {
                            str = str.Replace(strArray[0], "Leave");

                            dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"] = str;
                        }

                    }

                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"].ToString() != "")
                    {
                        char[] chrSplitter1 = { ':' };
                        string[] strArray1 = new string[2];
                        string str1 = dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"].ToString();
                        strArray1 = str1.Split(chrSplitter1);
                        if (strArray1[0].Length == 1)
                        {
                            strArray1[0] = "0" + strArray1[0];
                        }
                        if (strArray1[1].Length == 1)
                        {
                            strArray1[1] = "0" + strArray1[1];
                        }
                        str1 = strArray1[0] + ":" + strArray1[1];
                        dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"] = str1;

                    }

                }

                return dsSignInSignOutDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL.cs", "BindApprovalData", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }

        //edit
        //Get Status
        #region GetStatus
        public DataSet GetStatus()
        {


            try
            {
                dsSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetStatus");
                return dsSignInSignOutDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL.cs", "GetStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        public void UpdateStatus(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@SignInSignOutID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.SignInSignOutID;
                param[1] = new SqlParameter("@Status", SqlDbType.Int);
                param[1].Value = objSignInSignOutModel.StatusID;
                param[2] = new SqlParameter("@InTime", SqlDbType.DateTime);
                param[2].Value = objSignInSignOutModel.SignInTime;
                param[3] = new SqlParameter("@OutTime", SqlDbType.DateTime);
                if (objSignInSignOutModel.SignOutTime.ToString() == "1/1/0001 12:00:00 AM")
                {
                    param[3].Value = null;
                }
                else
                {
                    param[3].Value = objSignInSignOutModel.SignOutTime;
                }
                param[4] = new SqlParameter("@ApproversComments", SqlDbType.NVarChar);
                param[4].Value = objSignInSignOutModel.ApproverComments;
                SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SignInSignOutApprovalUpdate", param);

            }

            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "UpdateStatus", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }

        #endregion

        // Signin Signout Report

        #region SearchSigninSignOutRpt
        public DataSet SearchSigninSignOutRpt(SignInSignOutModel objSignInSignOutModel, bool IsAdmin, bool AllTeammembers)
        {
            try
            {
                DataSet dsSearchSigninSignOutRpt;
                SqlParameter[] objSqlParam = new SqlParameter[12];

                objSqlParam[0] = new SqlParameter("@StatusId", SqlDbType.Int);
                objSqlParam[0].Value = objSignInSignOutModel.StatusID;

                objSqlParam[1] = new SqlParameter("@UserId", SqlDbType.Int);
                objSqlParam[1].Value = objSignInSignOutModel.EmployeeID;

                objSqlParam[2] = new SqlParameter("@period ", SqlDbType.NVarChar);
                objSqlParam[2].Value = objSignInSignOutModel.Period;

                if (objSignInSignOutModel.FromDate.ToString() != "1/1/0001 12:00:00 AM")
                {
                    objSqlParam[3] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                    objSqlParam[3].Value = objSignInSignOutModel.FromDate;
                }
                else
                {
                    objSqlParam[3] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                    objSqlParam[3].Value = null;
                }
                if (objSignInSignOutModel.Todate.ToString() != "1/1/0001 12:00:00 AM")
                {
                    objSqlParam[4] = new SqlParameter("@Todate", SqlDbType.DateTime);
                    objSqlParam[4].Value = objSignInSignOutModel.Todate;
                }
                else
                {
                    objSqlParam[4] = new SqlParameter("@Todate", SqlDbType.DateTime);
                    objSqlParam[4].Value = null;
                }
                objSqlParam[5] = new SqlParameter("@Month", SqlDbType.NVarChar);
                objSqlParam[5].Value = objSignInSignOutModel.Month;

                objSqlParam[6] = new SqlParameter("@Year", SqlDbType.NVarChar);
                objSqlParam[6].Value = objSignInSignOutModel.Year;

                objSqlParam[7] = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                objSqlParam[7].Value = IsAdmin;

                objSqlParam[8] = new SqlParameter("@AllTeammembers", SqlDbType.Bit);
                objSqlParam[8].Value = AllTeammembers;

                objSqlParam[9] = new SqlParameter("@Type ", SqlDbType.Int);
                objSqlParam[9].Value = objSignInSignOutModel.Type;

                objSqlParam[10] = new SqlParameter("@Mode ", SqlDbType.Int);
                objSqlParam[10].Value = objSignInSignOutModel.Mode;

                objSqlParam[11] = new SqlParameter("@ShiftID ", SqlDbType.Int);
                objSqlParam[11].Value = objSignInSignOutModel.ShiftID;

                dsSearchSigninSignOutRpt = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SearchSignInSignOutRpt", objSqlParam);

                for (int i = 0; i < dsSearchSigninSignOutRpt.Tables[0].Rows.Count; i++)
                {

                    if (dsSearchSigninSignOutRpt.Tables[0].Rows[i]["TotalHoursWorked"].ToString() != "")
                    {
                        char[] chrSplitter1 = { ':' };
                        string[] strArray1 = new string[2];
                        string str1 = dsSearchSigninSignOutRpt.Tables[0].Rows[i]["TotalHoursWorked"].ToString();
                        strArray1 = str1.Split(chrSplitter1);
                        if (strArray1[0].Length == 1)
                        {
                            strArray1[0] = "0" + strArray1[0];
                        }
                        if (strArray1[1].Length == 1)
                        {
                            strArray1[1] = "0" + strArray1[1];
                        }
                        str1 = strArray1[0] + ":" + strArray1[1];
                        dsSearchSigninSignOutRpt.Tables[0].Rows[i]["TotalHoursWorked"] = str1;

                    }
                }


                return dsSearchSigninSignOutRpt;
            }

            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "SearchSigninSignOutRpt", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }

        }
        #endregion

        // Signin Signout Search

        #region SearchApproval
        public DataSet SearchApproval(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@EmployeeID", SqlDbType.Int);
                param[0].Value = objSignInSignOutModel.EmployeeID;
                param[1] = new SqlParameter("@Type", SqlDbType.Int);
                param[1].Value = objSignInSignOutModel.StatusID;
                param[2] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                param[2].Value = objSignInSignOutModel.Todate;
                param[3] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                param[3].Value = objSignInSignOutModel.FromDate;
                param[4] = new SqlParameter("@ColumnName", SqlDbType.NVarChar);
                param[4].Value = objSignInSignOutModel.ColumnName;
                param[5] = new SqlParameter("@SortOrder", SqlDbType.NVarChar);
                param[5].Value = objSignInSignOutModel.SortOrder;
                dsSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "SignInSignOutApprovalSearch", param);

                for (int i = 0; i < dsSignInSignOutDAL.Tables[0].Rows.Count; i++)
                {
                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["IsBulk"].ToString() == "True")
                    {
                        dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Bulk";
                    }
                    else
                    {
                        if (dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"].ToString() == "1")
                        {
                            dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Manual";
                        }
                        else if (dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"].ToString() == "0")
                        {
                            dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Auto";
                        }
                    }
                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString() != "")
                    {
                        string str = dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString();
                        char[] chrSplit = { ':' };
                        string[] strArray = new string[2];
                        strArray = str.Split(chrSplit);
                        if (strArray[0] == "##Leave")
                        {
                            str = str.Replace(strArray[0], "Leave");

                            dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"] = str;
                        }

                    }

                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"].ToString() != "")
                    {
                        char[] chrSplitter1 = { ':' };
                        string[] strArray1 = new string[2];
                        string str1 = dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"].ToString();
                        strArray1 = str1.Split(chrSplitter1);
                        if (strArray1[0].Length == 1)
                        {
                            strArray1[0] = "0" + strArray1[0];
                        }
                        if (strArray1[1].Length == 1)
                        {
                            strArray1[1] = "0" + strArray1[1];
                        }
                        str1 = strArray1[0] + ":" + strArray1[1];
                        dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"] = str1;

                    }

                }

                return dsSignInSignOutDAL;
            }

            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "SearchApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }
        #endregion

        //workflow
        public DataSet WFGetSignInSignOutDetails(int SignInSignOutID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@SignInSignOutID", SqlDbType.Int);
                param[0].Value = SignInSignOutID;
                dsSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "WFGetSignInSignOutDetails", param);
                return dsSignInSignOutDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL", "WFGetSignInSignOutDetails", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }


        #region UpdateEmployeeLeaveAndComp
        public int UpdateEmployeeLeaveAndComp(SignInSignOutModel objSignInSignOutModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@UserID", objSignInSignOutModel.EmployeeID);

                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "UpdateLeaveCompBalance", objParam);
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {


                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL.cs", "UpdateEmployeeLeaveAndComp", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);

            }
        }

        #endregion

        #region Admin Approval

        public DataSet GetSISOForAdminApproval(int StatusID, string FromDate, string ToDate, int UserID)
        {
            try
            {
                dsSignInSignOutDAL = null;
                
                SqlParameter[] param = new SqlParameter[4];               
                param[0] = new SqlParameter("@Status", StatusID);
                param[1] = new SqlParameter("@FromDate", FromDate);                
                param[2] = new SqlParameter("@Todate", ToDate);
                param[3] = new SqlParameter("@UserID", UserID);
                
                dsSignInSignOutDAL = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "GetSISOForAdminApproval", param);

                for (int i = 0; i < dsSignInSignOutDAL.Tables[0].Rows.Count; i++)
                {
                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["IsBulk"].ToString() == "True")
                    {
                        dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Bulk";
                    }
                    else
                    {
                        if (dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"].ToString() == "1")
                        {
                            dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Manual";
                        }
                        else if (dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"].ToString() == "0")
                        {
                            dsSignInSignOutDAL.Tables[0].Rows[i]["Mode"] = "Auto";
                        }
                    }
                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString() != "")
                    {
                        string str = dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"].ToString();
                        char[] chrSplit = { ':' };
                        string[] strArray = new string[2];
                        strArray = str.Split(chrSplit);
                        if (strArray[0] == "##Leave")
                        {
                            str = str.Replace(strArray[0], "Leave");

                            dsSignInSignOutDAL.Tables[0].Rows[i]["SignInComment"] = str;
                        }

                    }

                    if (dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"].ToString() != "")
                    {
                        char[] chrSplitter1 = { ':' };
                        string[] strArray1 = new string[2];
                        string str1 = dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"].ToString();
                        strArray1 = str1.Split(chrSplitter1);
                        if (strArray1[0].Length == 1)
                        {
                            strArray1[0] = "0" + strArray1[0];
                        }
                        if (strArray1[1].Length == 1)
                        {
                            strArray1[1] = "0" + strArray1[1];
                        }
                        str1 = strArray1[0] + ":" + strArray1[1];
                        dsSignInSignOutDAL.Tables[0].Rows[i]["TotalHoursWorked"] = str1;

                    }

                }

                return dsSignInSignOutDAL;
            }
            catch (V2Exceptions ex)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                FileLog objFileLog = FileLog.GetLogger();
                objFileLog.WriteLine(LogType.Error, ex.Message, "SignInSignOutDAL.cs", "GetSISOForAdminApproval", ex.StackTrace);
                throw new V2Exceptions(ex.ToString(),ex);
            }
        }



#endregion


    }
}
