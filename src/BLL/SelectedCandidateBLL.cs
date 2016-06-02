using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using BOL;
using System.Data;

namespace BLL
{
    public class SelectedCandidateBLL
    {
        SelectedCandidateDAL  objSelectedCandidateDAL = new SelectedCandidateDAL();

        DataSet dsSelectedCandidate = new DataSet();
       

        public DataSet GetSelectedCandidate(SelectedCandidateBOL objSelectedCandidateBOL)
        {
            dsSelectedCandidate = objSelectedCandidateDAL.GetSelectedCandidateRoundDetails(objSelectedCandidateBOL);
            return dsSelectedCandidate;
        }

        public DataSet GetGradeName()
        {
            dsSelectedCandidate = objSelectedCandidateDAL.GetGradeDetails();
            return dsSelectedCandidate;
        }

        public DataSet GetEmploymentType()
        {
            dsSelectedCandidate = objSelectedCandidateDAL.GetGetEmploymentTypeDetails();
            return dsSelectedCandidate;
        }

        public DataSet GetDesignationDetails()
        {
            dsSelectedCandidate = objSelectedCandidateDAL.GetDesignation();
            return dsSelectedCandidate;
        }
        public DataSet SetSelectedCandidate(SelectedCandidateBOL objSelectedCandidateBOL)
        {
            dsSelectedCandidate = objSelectedCandidateDAL.SetSelectedCandidateDetails(objSelectedCandidateBOL);
            return dsSelectedCandidate;
        }
        public DataSet GetCandidateScore(SelectedCandidateBOL objSelectedCandidateBOL)
        {
            dsSelectedCandidate = objSelectedCandidateDAL.GetCandidateScore(objSelectedCandidateBOL);
            return dsSelectedCandidate;
        }

        public DataSet GetDetailsformail(SelectedCandidateBOL objSelectedCandidateBOL)
        {
            dsSelectedCandidate = objSelectedCandidateDAL.GetDetailsformail(objSelectedCandidateBOL);
            return dsSelectedCandidate;
        }
    }
}
