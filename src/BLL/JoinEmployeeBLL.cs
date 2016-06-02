using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using BOL;
using System.Data;

namespace BLL
{
    public class JoinEmployeeBLL
    {     

        JoinEmployeeDAL objJoinEmployeeDAL = new JoinEmployeeDAL();      

        public Int64 GetLatestEmployeeCode(JoinEmployeeBOL objJoinEmployeeBOL)
        {
            Int64 latestEmployeeCode;
            latestEmployeeCode = objJoinEmployeeDAL.GetLatestEmployeeCode(objJoinEmployeeBOL);
            return latestEmployeeCode;
        }

        public DataSet CheckUserNameEmail(JoinEmployeeBOL objoinEmployeeBOL)
        {
            DataSet CheckUserNameEmail = new DataSet();
            CheckUserNameEmail = objJoinEmployeeDAL.CheckUserNameEmailAlreadyExist(objoinEmployeeBOL);
            return CheckUserNameEmail;

        }

        public DataSet CreaetNewEmployee(JoinEmployeeBOL objoinEmployeeBOL)
        {
            DataSet dsEmployeeCreated = new DataSet();
            dsEmployeeCreated = objJoinEmployeeDAL.CreateNewEmployee(objoinEmployeeBOL);
            return dsEmployeeCreated;

        }

    }
}
