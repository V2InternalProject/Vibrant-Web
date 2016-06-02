using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using BOL;
using System.Data;

namespace BLL
{
    public class RRFStatusBLL
    {
        RRFStatusDAL objRRFStatusDAL = new RRFStatusDAL();
        DataSet dsTooltipInfo = new DataSet();
        DataSet dsRRFStatus = new DataSet();
        DataSet dsRRFListToReassign = new DataSet();
        DataSet dsGetRRFNo = new DataSet();

        public DataSet GetRRFStatus(RRFStatusBOL objRRFStatusBOL)
        {
            dsRRFStatus = objRRFStatusDAL.GetRRFStatus(objRRFStatusBOL);
            return dsRRFStatus;
        }

        public DataSet GetTooltipInfo(RRFStatusBOL objRRFStatusBOL)
        {
            dsTooltipInfo = objRRFStatusDAL.GetTooltipInfo(objRRFStatusBOL);
            return dsTooltipInfo;
        }

        public void ReassignRRF(RRFStatusBOL objRRFStatusBOL)
        {
           objRRFStatusDAL.ReassignRRF(objRRFStatusBOL);          
        }
        public DataSet GetRRFListToReassign(RRFStatusBOL objRRFStatusBOL)
        {
            dsRRFListToReassign = objRRFStatusDAL.GetRRFListToReassign(objRRFStatusBOL);
            return dsRRFListToReassign;        
        }
        public DataSet GetRRFNo(RRFStatusBOL objRRFStatusBOL)
        {
            dsGetRRFNo=   objRRFStatusDAL.GetRRFNoDetails(objRRFStatusBOL);
            return dsGetRRFNo;
        }

    }
}
