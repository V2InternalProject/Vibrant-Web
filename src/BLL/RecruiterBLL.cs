using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using BOL;
using DAL;

namespace BLL
{
    public class RecruiterBLL
    {
        DataSet dsRRFDetails = new DataSet();
        DataSet dsGetRRFCodeList = new DataSet();
        RecruiterDAL objRecruiterDAL = new RecruiterDAL();

        public DataSet GetRRFDetails(int UserID)
        {
            dsRRFDetails = objRecruiterDAL.GetRRFDetails(UserID);
            return dsRRFDetails;
        }

        public void ChangeStatus(RecruiterBOL objRecruiterBOL)
        {
            objRecruiterDAL.ChangeStatus(objRecruiterBOL);
        }
        public DataSet SearchRRFCodeData(RecruiterBOL objRecruiterBOL)
        {
            return dsGetRRFCodeList = objRecruiterDAL.SearchRRFCodeData(objRecruiterBOL);
        }
    }

}
