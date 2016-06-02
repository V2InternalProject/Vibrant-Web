using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using BOL;

namespace DAL
{
    
    public class UtilityDAL
    {
        UtilityBOL objUtilityBOL = new UtilityBOL();

        public DataSet GetEmployeeInfo(UtilityBOL objUtilityBOL)
        {
            DataSet dsEmployeeInfo = new DataSet();

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@UserId", SqlDbType.Int);
            param[0].Value = objUtilityBOL.UserID;

            return dsEmployeeInfo = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetEmployeeInfo", param);

            
        }

    }
}
