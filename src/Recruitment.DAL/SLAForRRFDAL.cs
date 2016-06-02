using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BOL;
using System.Data.SqlClient;


namespace DAL
{
    public class SLAForRRFDAL
    {
        DataSet dsSLAForRRF = new DataSet();
        DataSet dsGetRRFNoDetails = new DataSet();

        public DataSet GetDataForSLA(SLAForRRFBOL objSLAForRRFBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@RRFNO", SqlDbType.Int);
            param[0].Value = objSLAForRRFBOL.RRFNo;

            return dsSLAForRRF = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetDataForSLA", param);
        }


        public DataSet GetRRFNoDetails(SLAForRRFBOL objSLAForRRFBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RRFId", SqlDbType.Int);
            param[0].Value = objSLAForRRFBOL.RRFNo;
            return dsGetRRFNoDetails = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRRFNoDetails", param);
        }
    }
}
