using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using BOL;

namespace DAL
{
    public class InterviewerDAL
    {
        InterviewerBOL objInterviewerBOL = new InterviewerBOL();

        public DataSet GetInterviewDetails(InterviewerBOL objInterviewerBOL)
        {

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Interviewer", SqlDbType.Int);
            param[0].Value = objInterviewerBOL.Interviewer;
           
            try
            {
                 return SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetInterviewDetails", param);
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
