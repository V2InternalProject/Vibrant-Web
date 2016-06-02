using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;
using BOL;

namespace BLL
{
    public class RRFListBLL
    {
        RRFListDAL objRRFListDAL = new RRFListDAL();
       // int countOfOfferIssedToAnyCandidate = 0;
        DataSet dsGetRRFForList = new DataSet();
        DataSet OfferIssedToAnyCandidate = new DataSet();

        public DataSet GetRRFForList(RRFListBOL objRRFListBOL)
        {
            return dsGetRRFForList = objRRFListDAL.GetRRFForList(objRRFListBOL);
        }
        public void CancelRRF(RRFApproverBOL objRRApproverBOL)
        {
            objRRFListDAL.CancelRRF(objRRApproverBOL);
        }

        public void CloseRRF(RRFListBOL objRRFListBOL)
        {
            objRRFListDAL.CloseRRF(objRRFListBOL);
        }

        public DataSet CheckOfferIssedToAnyCandidate(RRFListBOL objRRFListBOL)
        {
            return OfferIssedToAnyCandidate = objRRFListDAL.CheckOfferIssedToAnyCandidate(objRRFListBOL);
        }

        public void UdateRRFValuesToApprove(RRFListBOL objRRFListBOL)
        {
            objRRFListDAL.UdateRRFValuesToApprove(objRRFListBOL);
        }

        public DataSet SearchRRFNoData(RRFListBOL objRRfListBOL)
        {
            return dsGetRRFForList = objRRFListDAL.SearchRRFNoData(objRRfListBOL);
        }
    }
}
