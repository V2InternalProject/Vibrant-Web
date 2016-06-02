using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOL;
using DAL;
using System.Data; 

namespace BLL
{
  public   class RescheduledBLL
    {
        RescheduledDAL dal = new RescheduledDAL();
        RescheduledBOL objRescheduledBOL = new RescheduledBOL();

        DataSet dsRBOLDataset = new DataSet();
        public DataSet GetEmployeeName(string prefixText, string RoleName)
        {
            dsRBOLDataset = dal.GetEmployeeName(prefixText,RoleName );
            return dsRBOLDataset;
        }

        public DataSet SetCandidateSchecule(RescheduledBOL mod,int Roundnumber)
        {
            dsRBOLDataset = dal.SetCandidateSchecule(mod, Roundnumber);
            return dsRBOLDataset;

        }

        public DataSet GetDetailsformail(RescheduledBOL objRescheduledBOL)
        {
            dsRBOLDataset = dal.GetDetailsformail(objRescheduledBOL);
            return dsRBOLDataset;
        }
    }
}
