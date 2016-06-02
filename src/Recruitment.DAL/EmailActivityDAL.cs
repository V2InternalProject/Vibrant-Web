using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOL;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class EmailActivityDAL
    {
        DataSet dsEmailActivityDAL = new DataSet();
        public DataSet GetMailInfo(EmailActivityBOL objEmailActivityBOL)
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@ToID", SqlDbType.VarChar);
            param[0].Value = objEmailActivityBOL.ToID;
            param[1] = new SqlParameter("@FromID", SqlDbType.Int);
            param[1].Value = objEmailActivityBOL.FromID;
            param[2] = new SqlParameter("@CCID", SqlDbType.VarChar);
            param[2].Value = objEmailActivityBOL.CCID;
            param[3] = new SqlParameter("@EmailTemplateName", SqlDbType.VarChar);
            param[3].Value = objEmailActivityBOL.EmailTemplateName;
            return dsEmailActivityDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetMailInfo", param);

        }
    }
}
