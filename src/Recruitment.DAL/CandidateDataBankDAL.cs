using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using BOL;

namespace DAL
{
    public class CandidateDataBankDAL
    {
        CandidateDataBankBOL objCandidateDataBankBOL = new CandidateDataBankBOL();

        public DataSet GetCandidateStatus()
        {
            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateStatus");
        }

        public DataSet GetCandidateQualification()
        {
            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateQualification_Courses");
        }


        public DataSet GetCandidateSearchResults(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            SqlParameter[] param = new SqlParameter[8];

            param[0] = new SqlParameter("@name", SqlDbType.VarChar);
            if (objCandidateDataBankBOL.FirstName != null)
                param[0].Value = objCandidateDataBankBOL.FirstName;
            else
                param[0].Value = DBNull.Value;


            //param[1] = new SqlParameter("@work_ex", SqlDbType.VarChar);
            //if (candidatemodel.WorkExp!= null)
            //param[1].Value = candidatemodel.WorkExp;
            //else
            //param[1].Value = DBNull.Value;

            param[1] = new SqlParameter("@years", SqlDbType.Int);
            if (objCandidateDataBankBOL.Years != -1)
                param[1].Value = objCandidateDataBankBOL.Years;
            else
                param[1].Value = DBNull.Value;

            param[2] = new SqlParameter("@uptoyears", SqlDbType.Int);
            if (objCandidateDataBankBOL.UptoYears != -1)
                param[2].Value = objCandidateDataBankBOL.UptoYears;
            else
                param[2].Value = DBNull.Value;


            param[3] = new SqlParameter("@qualification", SqlDbType.Int);
            if (objCandidateDataBankBOL.Qualifications != -1)
                param[3].Value = objCandidateDataBankBOL.Qualifications;
            else
                param[3].Value = DBNull.Value;


            param[4] = new SqlParameter("@notice", SqlDbType.Int);
            if (objCandidateDataBankBOL.NoticePeriod != -1)
                param[4].Value = objCandidateDataBankBOL.NoticePeriod;
            else
                param[4].Value = DBNull.Value;


            param[5] = new SqlParameter("@status", SqlDbType.Int);
            if (objCandidateDataBankBOL.Status != -1)
                param[5].Value = objCandidateDataBankBOL.Status;
            else
                param[5].Value = DBNull.Value;

            param[6] = new SqlParameter("@SearchMode", SqlDbType.VarChar);
            param[6].Value = "SearchForDataBank";


            param[7] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[7].Value = DBNull.Value;

            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateSearchResults", param);

        }

        public DataSet DeleteCandidateSkillsByCandidateID(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = objCandidateDataBankBOL.CandidateID;

            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_DeleteCandidateSkillsByCandidateID", param);
        }

        public DataSet DeleteCandidateCertificationDetailsByCandidateID(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = objCandidateDataBankBOL.CandidateID;

            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_DeleteCandidateCertificationDetailsByCandidateID", param);
        }

        public DataSet DeleteCandidateEducationDetailsByCandidateID(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = objCandidateDataBankBOL.CandidateID;

            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_DeleteCandidateEducationDetailsByCandidateID_ChangeForDegree", param);
        }

        public DataSet DeleteCandidateExperienceDetailsByCandidateID(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = objCandidateDataBankBOL.CandidateID;

            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_DeleteCandidateExperienceDetailsByCandidateID_ChangeForType", param);
        }

        public DataSet DeleteCandidate(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@ID", SqlDbType.BigInt);
            param[0].Value = objCandidateDataBankBOL.CandidateID;

            return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_DeleteCandidate", param);
        }
    }
}
