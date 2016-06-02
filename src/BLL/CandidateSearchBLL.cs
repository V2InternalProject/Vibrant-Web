using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOL;
using DAL;


namespace BLL
{
    public class CandidateSearchBLL
    {
        DataSet dsCandidateDetails = new DataSet();
        CandidateSearchDAL objCandidateSearchDAL = new CandidateSearchDAL();
        CandidateSearchBOL objCandidateSearchBOL = new CandidateSearchBOL();

        public DataSet GetCandidateStatus()
        {
            dsCandidateDetails = objCandidateSearchDAL.GetCandidateStatus();
            return dsCandidateDetails;
        }

        public DataSet GetCandidateQualification()
        {
            dsCandidateDetails = objCandidateSearchDAL.GetCandidateQualification();
            return dsCandidateDetails;
        }

        public DataSet GetCandidateSearchResults(CandidateSearchBOL objCandidateSearchBOL)
        {
            dsCandidateDetails = objCandidateSearchDAL.GetCandidateSearchResults(objCandidateSearchBOL);
            return dsCandidateDetails;
        }

        public void ChangeCandidateStatus(int CandidateID, int RRFID)
        {

            objCandidateSearchDAL.ChangeCandidateStatus(CandidateID, RRFID);    
        }

    }
}
