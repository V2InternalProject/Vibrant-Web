using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOL;
using DAL;

namespace BLL
{
    public class CandidateBLL
    {
        CandidateDAL candidateDAL = new CandidateDAL();
        DataSet dsCandidateBLL = new DataSet();



        public DataSet AddCandidate(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.AddCandidate(candidateBOL);
            return dsCandidateBLL;

        }

        public DataSet UpdateCandidate(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.UpdateCandidate(candidateBOL);
            return dsCandidateBLL;
        }

        public DataSet UpdateCandidateFileUploadStatus(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.UpdateCandidateFileUploadStatus(candidateBOL);
            return dsCandidateBLL;

        }

        public DataSet GetCandidateDetails(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.GetCandidateDetails(candidateBOL);
            return dsCandidateBLL;

        }
        public DataSet GetCandidateHistory(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.GetCandidateHistory(candidateBOL);
            return dsCandidateBLL;

        }
        public DataSet GetCandidateDetailsByFirstNameAndLastNameAndDOB(string firstName, string lastname, string DOB)
        {
            dsCandidateBLL = candidateDAL.GetCandidateDetailsByFirstNameAndLastNameAndDOB(firstName, lastname, DOB);
            return dsCandidateBLL;
        }


        public DataSet AddCandidateExperienceDetails(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.AddCandidateExperienceDetails(candidateBOL);
            return dsCandidateBLL;
        }


        public DataSet AddCandidateCertificationDetails(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.AddCandidateCertificationDetails(candidateBOL);
            return dsCandidateBLL;
        }

        public DataSet AddCandidateEducationDetails(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.AddCandidateEducationDetails(candidateBOL);
            return dsCandidateBLL;
        }

        public DataSet UpdateCandidateExperienceDetails(int ID, CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.UpdateCandidateExperienceDetails(ID, candidateBOL);
            return dsCandidateBLL;
        }

        public DataSet UpdateCandidateCertificationDetails(int ID, CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.UpdateCandidateCertificationDetails(ID, candidateBOL);
            return dsCandidateBLL;
        }

        public DataSet UpdateCandidateEducationDetails(int ID, CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.UpdateCandidateEducationDetails(ID, candidateBOL);
            return dsCandidateBLL;

        }


        public DataSet DeleteCandidateExperienceDetails(int ID)
        {
            dsCandidateBLL = candidateDAL.DeleteCandidateExperienceDetails(ID);
            return dsCandidateBLL;
        }

        public DataSet DeleteCandidateCertificationDetails(int ID)
        {
            dsCandidateBLL = candidateDAL.DeleteCandidateCertificationDetails(ID);
            return dsCandidateBLL;
        }

        public DataSet DeleteCandidateEducationDetails(int ID)
        {
            dsCandidateBLL = candidateDAL.DeleteCandidateEducationDetails(ID);
            return dsCandidateBLL;
        }

        public DataSet DeleteCandidateSkills(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.DeleteCandidateSkills(candidateBOL);
            return dsCandidateBLL;
        }

        public DataSet AddCandidateSkills(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.AddCandidateSkills(candidateBOL);
            return dsCandidateBLL;

        }

        public DataSet UpdateCandidateSkills(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.UpdateCandidateSkills(candidateBOL);
            return dsCandidateBLL;
        }

        public string GetLatestExperienceID(CandidateBOL candidateBOL)
        {
            return candidateDAL.GetLatestExperienceID(candidateBOL);
        }

        public DataSet GetCandidateExpID(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.GetCandidateExpID(candidateBOL);
            return dsCandidateBLL;
        }

        public DataSet GetCandidateCertificationID(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.GetCandidateCertificationID(candidateBOL);
            return dsCandidateBLL;
        }

        public DataSet GetCandidateEducationID(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.GetCandidateEducationID(candidateBOL);
            return dsCandidateBLL;
        }

        public string GetLatestCertificationID(CandidateBOL candidateBOL)
        {
            return candidateDAL.GetLatestCertificationID(candidateBOL);
        }

        public string GetLatestEducationID(CandidateBOL candidateBOL)
        {
            return candidateDAL.GetLatestEducationID(candidateBOL);
        }

        public DataSet GetCandidateSkills(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.GetCandidateSkills(candidateBOL);
            return dsCandidateBLL;
        }

        //public DataSet GetAllEstablishment()
        //{
        //    dsCandidateBLL = candidateDAL.GetAllEstablishment();
        //    return dsCandidateBLL;
        //}

        public DataSet GetAllOrganization()
        {
            dsCandidateBLL = candidateDAL.GetAllOrganization();
            return dsCandidateBLL;
        }

        public DataSet GetExperienceDetails(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.GetExperienceDetails(candidateBOL);
            return dsCandidateBLL;
        }

        public DataSet GetExperienceDetails(string candidateID)
        {
            dsCandidateBLL = candidateDAL.GetExperienceDetails(candidateID);
            return dsCandidateBLL;
        }


        public DataSet GetEducationDetails(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.GetEducationDetails(candidateBOL);
            return dsCandidateBLL;
        }

        public DataSet GetEducationDetails(string candidateID)
        {
            dsCandidateBLL = candidateDAL.GetEducationDetails(candidateID);
            return dsCandidateBLL;
        }

        public DataSet GetCertificationDetails(CandidateBOL candidateBOL)
        {
            dsCandidateBLL = candidateDAL.GetCertificationDetails(candidateBOL);
            return dsCandidateBLL;
        }

        public DataSet GetCertificationDetails(string candidateID)
        {
            dsCandidateBLL = candidateDAL.GetCertificationDetails(candidateID);
            return dsCandidateBLL;
        }

        public DataSet GetAllCourses()
        {
            return dsCandidateBLL = candidateDAL.GetAllCourses();

        }

        public DataSet GetAllCountryNames()
        {
            return dsCandidateBLL = candidateDAL.GetAllCountryNames();

        }

        public DataSet GetAllCertificationNames()
        {
            return dsCandidateBLL = candidateDAL.GetAllCertificationNames();

        }

        public DataSet GetAllDegree()
        {
            return dsCandidateBLL = candidateDAL.GetAllDegree();

        }

        //public DataSet GetAllCoursesByPGUG(string PGUG)
        //{
        //    return dsCandidateBLL = candidateDAL.GetAllCoursesByPGUG(PGUG);
        //}

        public DataSet GetAllCourseTypes()
        {
            return dsCandidateBLL = candidateDAL.GetAllCourseTypes();

        }

        public DataSet GetAllExpTypes()
        {
            return dsCandidateBLL = candidateDAL.GetAllExpTypes();

        }
        public DataSet GetAllSkills(string mode)
        {
            return dsCandidateBLL = candidateDAL.GetAllSkills(mode);
        }

        public DataSet GetAllStaus()
        {
            return dsCandidateBLL = candidateDAL.GetAllStaus();
        }

    }
}
