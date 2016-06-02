using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL;
using BOL;

namespace BLL
{
    public class SLAForRRFBLL
    {
        DataSet dsSLAForRRF = new DataSet();
        DataSet dsGetRRFNo = new DataSet();
        SLAForRRFDAL objSLAForRRFDAL = new SLAForRRFDAL();

        public DataSet GetDataForSLA(SLAForRRFBOL objSLAForRRFBOL)
        {
            dsSLAForRRF = objSLAForRRFDAL.GetDataForSLA(objSLAForRRFBOL);
            return dsSLAForRRF;
        }

        public DataSet GetRRFNo(SLAForRRFBOL objSLAForRRFBOL)
        {
            dsGetRRFNo = objSLAForRRFDAL.GetRRFNoDetails(objSLAForRRFBOL);
            return dsGetRRFNo;
        }
    }
}
