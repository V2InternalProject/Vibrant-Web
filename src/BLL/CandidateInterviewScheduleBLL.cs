using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOL;
using DAL;

namespace BLL
{
    public class CandidateInterviewScheduleBLL
    {
        CandidateInterviewScheduleDAL dal = new CandidateInterviewScheduleDAL();
        DataSet dsSBOLDataset = new DataSet();

        public DataSet GetRRFValues(CandidateInterviewScheduleBOL mod, int CandidateID)
        {
            dsSBOLDataset = dal.GetRRFValues(mod, CandidateID);
            return dsSBOLDataset;
        }

        public DataSet GetStage(CandidateInterviewScheduleBOL mod)
        {
            dsSBOLDataset = dal.GetStage(mod);
            return dsSBOLDataset;
        }

        public DataSet GetCandidateSchedule(int CandidateID, int RRFID)
        {
            dsSBOLDataset = dal.GetCandidateSchedule(CandidateID, RRFID);
            return dsSBOLDataset;
        }


        public DataSet GetCandidateReScheduleRoundNumber(int CandidateID, int RRFID,int RoundNo)
        {
            dsSBOLDataset = dal.GetCandidateReScheduleRoundNumber(CandidateID, RRFID, RoundNo);
            return dsSBOLDataset;
        }



        public void SetCandidateSchecule(CandidateInterviewScheduleBOL mod, int RowNumberForReschedule)
        {
            dal.SetCandidateSchecule(mod,RowNumberForReschedule );
            
        }
        public DataSet GetEmployeeName(string prefixText,string RoleName)
        {
            dsSBOLDataset = dal.GetRoleBasedInterViewer(prefixText,RoleName );
            return dsSBOLDataset;
        }





        public DataSet GetDetailsformail(CandidateInterviewScheduleBOL objCandidateInterviewScheduleBOL)
        {
            dsSBOLDataset = dal.GetDetailsformail(objCandidateInterviewScheduleBOL);
            return dsSBOLDataset;
        }
    }
}
