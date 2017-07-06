using BOL;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System;

namespace DAL
{
    public class RRFListDAL
    {
        DataSet dsGetRRFForList = new DataSet();
        DataSet OfferIssedToAnyCandidate = new DataSet();
        //int countOfOfferIssedToAnyCandidate = 0;

        public DataSet GetRRFForList(RRFListBOL objRRFListBOL)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@UserId", SqlDbType.Int);
            param[0].Value = objRRFListBOL.UserID;
            param[1] = new SqlParameter("@RoleType", SqlDbType.VarChar);
            param[1].Value = objRRFListBOL.RoleType;
            return dsGetRRFForList = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRRFList", param);
        }

        public DataSet CheckOfferIssedToAnyCandidate(RRFListBOL objRRFListBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RRFId", SqlDbType.Int);
            param[0].Value = objRRFListBOL.RRFID;
            return OfferIssedToAnyCandidate = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_CheckOfferissedtoAnyCandidate", param);
            //return OfferIssedToAnyCandidate = Convert.ToInt32(SqlHelper.ExecuteScalar(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_CheckOfferissedtoAnyCandidate", param));
        }

        public void CancelRRF(RRFApproverBOL objRRFApproverBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RRFId", SqlDbType.Int);
            param[0].Value = objRRFApproverBOL.RRFID;

            SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_CancelRRF", param);
        }

        public void CloseRRF(RRFListBOL objRRFListBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RRFId", SqlDbType.Int);
            param[0].Value = objRRFListBOL.RRFID;

            SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_CloseRRF", param);
        }

        public void UdateRRFValuesToApprove(RRFListBOL objRRFListBOL)
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[0].Value = objRRFListBOL.RRFID;

            param[1] = new SqlParameter("@Comments", SqlDbType.VarChar);
            param[1].Value = DBNull.Value;

            param[2] = new SqlParameter("@RRFStatus", SqlDbType.Int);
            param[2].Value = objRRFListBOL.RRFStatus;

            param[3] = new SqlParameter("@ApprovalStatus", SqlDbType.Int);
            param[3].Value = objRRFListBOL.ApprovalStatus;

            param[4] = new SqlParameter("@ModifiedBy", SqlDbType.Int);
            param[4].Value = objRRFListBOL.UserID;

            param[5] = new SqlParameter("@ModifiedDate", SqlDbType.DateTime);
            param[5].Value = objRRFListBOL.ModifiedDate;

            SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_UdateRRFValuesToApprove", param);
        }

        public DataSet SearchRRFNoData(RRFListBOL objRRFListBOL)
        {
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@UserId", SqlDbType.Int);
            param[0].Value = objRRFListBOL.UserID;
            param[1] = new SqlParameter("@RoleType", SqlDbType.VarChar);
            param[1].Value = objRRFListBOL.RoleType;
            param[2] = new SqlParameter("@RRFNo", SqlDbType.VarChar);
            param[2].Value = objRRFListBOL.RRFNo;
            param[3] = new SqlParameter("@RRFStatus", SqlDbType.Int);
            param[3].Value = objRRFListBOL.RRFStatus;
            return dsGetRRFForList = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_RRFNoSearch", param);
        }

        public DataSet RRFStatus()
        {
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.Text, "select ID,RRFStatus from tbl_RRFStatus");
            return ds;
        }
    }
}
