using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOL;
using DAL;

namespace BLL
{
    public class CandidateDataBankBLL
    {
        DataSet dsCandidateDetails = new DataSet();
        CandidateDataBankDAL objCandidateDataBankDAL = new CandidateDataBankDAL();
        CandidateDataBankBOL objCandidateDataBankBOL = new CandidateDataBankBOL();

        public DataSet GetCandidateStatus()
        {
            dsCandidateDetails = objCandidateDataBankDAL.GetCandidateStatus();
            return dsCandidateDetails;
        }

        public DataSet GetCandidateQualification()
        {
            dsCandidateDetails = objCandidateDataBankDAL.GetCandidateQualification();
            return dsCandidateDetails;
        }

        public DataSet GetCandidateSearchResults(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            dsCandidateDetails = objCandidateDataBankDAL.GetCandidateSearchResults(objCandidateDataBankBOL);
            return dsCandidateDetails;
        }

        public DataSet DeleteCandidateSkillsByCandidateID(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            dsCandidateDetails = objCandidateDataBankDAL.DeleteCandidateSkillsByCandidateID(objCandidateDataBankBOL);
            return dsCandidateDetails;
        }

        public DataSet DeleteCandidateCertificationDetailsByCandidateID(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            dsCandidateDetails = objCandidateDataBankDAL.DeleteCandidateCertificationDetailsByCandidateID(objCandidateDataBankBOL);
            return dsCandidateDetails;
        }

        public DataSet DeleteCandidateEducationDetailsByCandidateID(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            dsCandidateDetails = objCandidateDataBankDAL.DeleteCandidateEducationDetailsByCandidateID(objCandidateDataBankBOL);
            return dsCandidateDetails;
        }

        public DataSet DeleteCandidateExperienceDetailsByCandidateID(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            dsCandidateDetails = objCandidateDataBankDAL.DeleteCandidateExperienceDetailsByCandidateID(objCandidateDataBankBOL);
            return dsCandidateDetails;
        }

        public DataSet DeleteCandidate(CandidateDataBankBOL objCandidateDataBankBOL)
        {
            dsCandidateDetails = objCandidateDataBankDAL.DeleteCandidate(objCandidateDataBankBOL);
            return dsCandidateDetails;
        }

    }
}
