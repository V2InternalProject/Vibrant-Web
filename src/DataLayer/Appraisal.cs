using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Text;
using V2.CommonServices.Exceptions;
using System.Data.SqlClient;

namespace V2.Orbit.DataLayer
{
    [Serializable]
    public class Appraisal : DBBaseClass
    {
        public Dictionary<int, string> GetAppraisalYear()
        {
            Dictionary<int, string> dAppraisalYear = new Dictionary<int, string>();
            try
            {
                DataSet Year;

                Year = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "getAppraisalYear");

                if (Year != null && Year.Tables.Count > 0)
                {
                    if (Year.Tables[0] != null && Year.Tables[0].Rows.Count > 0)
                    {
                        int ID = Convert.ToInt16(Year.Tables[0].Rows[0]["AppraisalYearID"]);
                        string appYear = Convert.ToString(Year.Tables[0].Rows[0]["AppraisalYear"]);

                        dAppraisalYear.Add(ID, appYear);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
            return dAppraisalYear;
        }

        public DataSet GetSectionList(int? dYearId)
        {
            DataSet dsSection;
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@yearId", dYearId);

                dsSection = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "getAppraisalSectionList", objParam);
                return dsSection;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

        public DataSet GetQuestionList(int? yearID,int? sectionID)
        {
            DataSet dsQuestion;
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@yearId", yearID);
                objParam[1] = new SqlParameter("@sectionId", sectionID);

                dsQuestion = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "getAppraisalQuestionList", objParam);
                return dsQuestion;
            }
            catch (Exception ex)
            {
                throw new V2Exceptions(ex.ToString(), ex);
            }
        }

    }
}
