using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOL;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace DAL
{
    public class RRFStatusDAL
    {
        DataSet dsGetRRFStatusDAL = new DataSet();
        DataSet dsGetTooltipInfoDAL = new DataSet();
        DataSet dsGetRRFNoDetails = new DataSet();
        RRFStatusBOL objRRFStatusBOL = new RRFStatusBOL();
        
        public DataSet GetRRFStatus(RRFStatusBOL objRRFStatusBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[0].Value = objRRFStatusBOL.RRFNo; 
            return dsGetRRFStatusDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateProgress",param);
        }

        public DataSet GetRRFListToReassign(RRFStatusBOL objRRFStatusBOL)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@Recruiter", SqlDbType.Int);
            param[0].Value = objRRFStatusBOL.RecruiterID;
            param[1] = new SqlParameter("@CurrentRRF", SqlDbType.Int);
            param[1].Value = objRRFStatusBOL.RRFNo;
            return dsGetRRFStatusDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRRFToReassign", param);
        }

        public DataSet GetTooltipInfo(RRFStatusBOL objRRFStatusBOL)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = objRRFStatusBOL.CandidateID;
            param[1] = new SqlParameter("@RRFNo", SqlDbType.Int);
            param[1].Value = objRRFStatusBOL.RRFNo; 
            return dsGetRRFStatusDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetTooltipInfo", param);
        }
        public void ReassignRRF(RRFStatusBOL objRRFStatusBOL)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = objRRFStatusBOL.CandidateID;
            param[1] = new SqlParameter("@NewRRF", SqlDbType.Int);
            param[1].Value = objRRFStatusBOL.RRFNo;
            SqlHelper.ExecuteNonQuery(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_ReassignCandidateToRRF", param);
      
        }

        public DataSet GetRRFNoDetails(RRFStatusBOL objRRFStatusBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RRFId", SqlDbType.Int);
            param[0].Value = objRRFStatusBOL.RRFNo;
            return dsGetRRFNoDetails = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRRFNoDetails", param);
        }
    }
}
