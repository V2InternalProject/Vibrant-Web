using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;
using BOL;

namespace BLL
{
    public class HRInterviewAssessmentBLL
    {
        DataSet dsCandidateDetails = new DataSet();
        HRInterviewAssessmentDAL objHRInterviewAssessmentDAL = new HRInterviewAssessmentDAL();

        public DataSet GetCandidateDetails(HRInterviewAssessmentBOL objHRInterviewAssessmentBOL)
        {
            dsCandidateDetails = objHRInterviewAssessmentDAL.GetCandidateDetails( objHRInterviewAssessmentBOL);
            return dsCandidateDetails;

        }
        public void HRInterviewAssessment(HRInterviewAssessmentBOL objHRInterviewAssessmentBOL)
        {
            objHRInterviewAssessmentDAL.HRInterviewAssessment(objHRInterviewAssessmentBOL);
        }

        public void UpdateCandidateScheduleDate(HRInterviewAssessmentBOL objHRInterviewAssessmentBOL)
        {
            objHRInterviewAssessmentDAL.GetUpdateCandidateScheduleDate(objHRInterviewAssessmentBOL);
        }

        public DataSet GetDetailsformail(HRInterviewAssessmentBOL objHRInterviewAssessmentBOL)
        {
            dsCandidateDetails = objHRInterviewAssessmentDAL.GetDetailsformail(objHRInterviewAssessmentBOL);
            return dsCandidateDetails;
        }
    }
}
