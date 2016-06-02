using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using BOL;
using System.Data;

namespace BLL
{    
    public class HRMBLL
    {
        DataSet dsRRFDetails = new DataSet();
        DataSet dsRecruiterNames = new DataSet();
        DataSet dsSLAType = new DataSet();

        int countOfOfferIssedToAnyCandidate = 0;

        HRMDAL objHRMDAL = new HRMDAL();
        HRMBOL objHRMBOL = new HRMBOL();

        public DataSet GetRRFToApprove(HRMBOL objHRMBOL)
        {
            dsRRFDetails = objHRMDAL.GetRRFToApprove(objHRMBOL);
            return dsRRFDetails;
        }

        public DataSet GetRecruiterNames()
        {
            dsRecruiterNames = objHRMDAL.GetRecruiterNames();
            return dsRecruiterNames;
        }

        public void SetRecruiterToRRF(HRMBOL objHRMBOL)
        {
            objHRMDAL.SetRecruiterToRRF(objHRMBOL);
         
        }
        public int CheckOfferIssedToAnyCandidate(HRMBOL objHRMBOL)
        {
            return countOfOfferIssedToAnyCandidate = objHRMDAL.CheckOfferIssedToAnyCandidate(objHRMBOL);
        }
        public DataSet GetSLAType()
        {
            dsSLAType = objHRMDAL.GetSLAType();
            return dsSLAType;
        }
    }
}
