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
    public class HRMDAL
    {
        HRMBOL objHRMBOL = new HRMBOL();
        int countOfOfferIssedToAnyCandidate = 0;

        public int CheckOfferIssedToAnyCandidate(HRMBOL objHRMBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RRFId", SqlDbType.Int);
            param[0].Value = objHRMBOL.RRFID;

            return countOfOfferIssedToAnyCandidate = Convert.ToInt32(SqlHelper.ExecuteScalar(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_CheckOfferissedtoAnyCandidate", param));
        }

        public DataSet GetRRFToApprove(HRMBOL objHRMBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[0].Value = objHRMBOL.RRFID;
           
            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRRFToApprove",param);
        }

        public DataSet GetRecruiterNames()
        {
            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRecruiterNames");
        }
        public DataSet GetSLAType()
        {
            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_getSLAType");
        }

        public void SetRecruiterToRRF(HRMBOL objHRMBOL)
        {
            SqlParameter[] param = new SqlParameter[7];

            param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[0].Value = objHRMBOL.RRFID;

            param[1] = new SqlParameter("@RecruiterID", SqlDbType.Int);
            param[1].Value = objHRMBOL.RecruiterID;

            param[2] = new SqlParameter("@AssignedBy", SqlDbType.Int);
            param[2].Value = objHRMBOL.AssignedBy;

            param[3] = new SqlParameter("@AssignedDate", SqlDbType.DateTime);
            param[3].Value = objHRMBOL.AssignedDate;

            param[4] = new SqlParameter("@ModifiedDate", SqlDbType.DateTime);
            param[4].Value = objHRMBOL.ModifiedDate;

            param[5] = new SqlParameter("@ModifiedBy", SqlDbType.Int);
            param[5].Value = objHRMBOL.ModifiedBy;

            param[6] = new SqlParameter("@SLAType", SqlDbType.Int);
            param[6].Value = objHRMBOL.SLAType;
            

            SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_SetRecruiterToRRF",param);
        }
    }
}
