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
    public class CandidateSearchDAL
    {
        CandidateSearchBOL objCandidateSearchBOL = new CandidateSearchBOL();

        public DataSet GetCandidateStatus()
        {
            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateStatus");
        }

        public DataSet GetCandidateQualification()
        {
            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateQualification_Courses");
        }

        public void ChangeCandidateStatus(int CandidateID, int RRFID)
        {
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.VarChar);
            param[0].Value = CandidateID;


            param[1] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[1].Value = RRFID;

            SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_ChangeCandidateStatus", param);

        }

        public DataSet GetCandidateSearchResults(CandidateSearchBOL objCandidateSearchBOL)
        {
            SqlParameter[] param = new SqlParameter[8];

            param[0] = new SqlParameter("@name", SqlDbType.VarChar);
            if (objCandidateSearchBOL.Name != null)
                param[0].Value = objCandidateSearchBOL.Name;
            else
                param[0].Value = DBNull.Value;


            //param[1] = new SqlParameter("@work_ex", SqlDbType.VarChar);
            //if (candidatemodel.WorkExp!= null)
            //param[1].Value = candidatemodel.WorkExp;
            //else
            //param[1].Value = DBNull.Value;

            param[1] = new SqlParameter("@years", SqlDbType.Int);
            if (objCandidateSearchBOL.Years != -1)
                param[1].Value = objCandidateSearchBOL.Years;
            else
                param[1].Value = DBNull.Value;

            param[2] = new SqlParameter("@uptoyears", SqlDbType.Int);
            if (objCandidateSearchBOL.UptoYears != -1)
                param[2].Value = objCandidateSearchBOL.UptoYears;
            else
                param[2].Value = DBNull.Value;


            param[3] = new SqlParameter("@qualification", SqlDbType.Int);
            if (objCandidateSearchBOL.Qualifications != -1)
                param[3].Value = objCandidateSearchBOL.Qualifications;
            else
                param[3].Value = DBNull.Value;


            param[4] = new SqlParameter("@notice", SqlDbType.Int);
            if (objCandidateSearchBOL.NoticePeriod != -1)
                param[4].Value = objCandidateSearchBOL.NoticePeriod;
            else
                param[4].Value = DBNull.Value;


            param[5] = new SqlParameter("@status", SqlDbType.Int);
            if (objCandidateSearchBOL.Status != -1)
                param[5].Value = objCandidateSearchBOL.Status;
            else
                param[5].Value = DBNull.Value;

            param[6] = new SqlParameter("@SearchMode", SqlDbType.VarChar);                
            param[6].Value = "SearchForRecruiter";

            param[7] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[7].Value = objCandidateSearchBOL.RRFID;

            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateSearchResults", param);

        }

    }
}
