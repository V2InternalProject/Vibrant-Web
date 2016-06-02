using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOL;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class JoinEmployeeDAL
    {
        InterviewerBOL objJoinedEmployeeBLL = new InterviewerBOL();
        public Int64 GetLatestEmployeeCode(JoinEmployeeBOL objJoinEmployeeBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@IsContract", SqlDbType.Int);
            param[0].Value = objJoinEmployeeBOL.IsContract;
            return Convert.ToInt64(SqlHelper.ExecuteScalar(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "GetLatestEmployeecode",param));

        }

        public DataSet CreateNewEmployee(JoinEmployeeBOL objoinEmployeeBOL)
        {
            DataSet dsEmployeeCreated;
            SqlParameter[] param = new SqlParameter[5];

            param[0] = new SqlParameter("@EmployeeCode", SqlDbType.Int);
            param[0].Value = objoinEmployeeBOL.Employeecode;

            param[1] = new SqlParameter("@UserName", SqlDbType.NVarChar);
            param[1].Value = objoinEmployeeBOL.UserName;

            param[2] = new SqlParameter("@EmailId", SqlDbType.NVarChar);
            param[2].Value = objoinEmployeeBOL.EmailId;

            param[3] = new SqlParameter("@CandidateId", SqlDbType.Int);
            param[3].Value = objoinEmployeeBOL.CandidateId;

            param[4] = new SqlParameter("@JoiningDate", SqlDbType.DateTime);
            param[4].Value = objoinEmployeeBOL.JoiningDate;

            return dsEmployeeCreated = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "AddNewEmployee", param);

        }

        public DataSet CheckUserNameEmailAlreadyExist(JoinEmployeeBOL objoinEmployeeBOL)
        {
            DataSet CheckUserNameEmail = new DataSet();
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@UserName", SqlDbType.NVarChar);
            param[0].Value = objoinEmployeeBOL.UserName;

            param[1] = new SqlParameter("@EmailId", SqlDbType.NVarChar);
            param[1].Value = objoinEmployeeBOL.EmailId;

            return CheckUserNameEmail = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "CheckUserNameEmailExist", param);

        }
    }
}
