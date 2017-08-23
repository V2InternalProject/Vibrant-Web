using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using BOL;

namespace DAL
{
    public class RecruiterDAL
    {
        DataSet dsGetRRFCodeList = new DataSet();

        public DataSet GetRRFDetails(int UserID)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@UserID", SqlDbType.Int);
            param[0].Value = UserID;
            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRecruiterHeader", param);
        }

        public void ChangeStatus(RecruiterBOL objRecruiterBOL)
        {
            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@RRFID", SqlDbType.VarChar);
            param[0].Value = objRecruiterBOL.RRFID;

            param[1] = new SqlParameter("@ModifiedDate", SqlDbType.DateTime);
            param[1].Value = objRecruiterBOL.ModifiedDate;

            param[2] = new SqlParameter("@ModifiedBy", SqlDbType.Int);
            param[2].Value = objRecruiterBOL.ModifiedBy;

            param[3] = new SqlParameter("@RRFAcceptedDate", SqlDbType.DateTime);
            param[3].Value = objRecruiterBOL.RRFAcceptedDate;

            SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_ChangeRRFStatus", param);

        }

        public DataSet SearchRRFCodeData(RecruiterBOL objRecruiterBOL)
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@UserId", SqlDbType.Int);
            param[0].Value = objRecruiterBOL.UserID;
            param[1] = new SqlParameter("@RoleType", SqlDbType.VarChar);
            param[1].Value = objRecruiterBOL.RoleType;
            param[2] = new SqlParameter("@RRFNo", SqlDbType.VarChar);
            param[2].Value = objRecruiterBOL.RRFCode;
            param[3] = new SqlParameter("@RRFStatus", SqlDbType.Int);
            param[3].Value = 0;
            return dsGetRRFCodeList = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_RRFNoSearch", param);
        }

    }
}
