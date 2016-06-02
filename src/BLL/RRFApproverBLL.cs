using System.Linq;
using System.Text;
using System.Data;
using Recruitment.DAL;
using BOL;
using DAL;

namespace BLL
{
    public class RRFApproverBLL
    {
        RRFApproverDAL objRRFApproverDAL = new RRFApproverDAL();
        DataSet dsGetRRFValuesToApprove = new DataSet();
        DataSet dsGetDeliveryUnit = new DataSet();
        DataSet dsGetDesignation = new DataSet();
        DataSet dsGetDeliveryTeam = new DataSet();
        DataSet dsGetResourcePool = new DataSet();
        DataSet dsGetEmployeeName = new DataSet();
        DataSet dsGetEmploymentType = new DataSet();
        DataSet dsAddRRF = new DataSet();
        DataSet dsGetEmployeeFromRole = new DataSet();
        DataSet dsSLAForTechnology = new DataSet();
        DataSet dsTotalSLADaysForTech = new DataSet();
        DataSet dsmaildetails = new DataSet();

        public void UdateRRFValuesToApprove(RRFApproverBOL objRRFApproverBOL)
        {
            objRRFApproverDAL.UdateRRFValuesToApprove(objRRFApproverBOL);
        }

        public DataSet GetRRFValuesToApprove(RRFApproverBOL objRRFApproverBOL)
        {
            dsGetRRFValuesToApprove = objRRFApproverDAL.GetRRFValuesToApprove(objRRFApproverBOL);
            return dsGetRRFValuesToApprove;
        }


        public DataSet GetDeliveryUnit(RRFApproverBOL objRRFApproverBOL)
        {
            dsGetDeliveryUnit = objRRFApproverDAL.GetDeliveryUnit(objRRFApproverBOL);
            return dsGetDeliveryUnit;
        }

        public DataSet GetDeliveryTeam(int deliveryUnit)
        {
            dsGetDeliveryTeam = objRRFApproverDAL.GetDeliveryTeam(deliveryUnit);
            return dsGetDeliveryTeam;
        }

        public DataSet GetDesignation(RRFApproverBOL objRRFApproverBOL)
        {
            dsGetDesignation = objRRFApproverDAL.GetDesignation(objRRFApproverBOL);
            return dsGetDesignation;
        }

        public DataSet GetResourcePool(RRFApproverBOL objRRFApproverBOL)
        {
            dsGetResourcePool = objRRFApproverDAL.GetResourcePool(objRRFApproverBOL);
            return dsGetResourcePool;
        }

        public DataSet GetEmployeeName(string prefixText)
        {
            dsGetEmployeeName = objRRFApproverDAL.GetEmployeeName(prefixText);
            return dsGetEmployeeName;
        }

        public DataSet GetEmploymentType(RRFApproverBOL objRRFApproverBOL)
        {
            dsGetEmploymentType = objRRFApproverDAL.GetEmploymentType(objRRFApproverBOL);
            return dsGetEmploymentType;
        }

        public void AddRRF(RRFApproverBOL objRRFApproverBOL)
        {
            objRRFApproverDAL.AddRRF(objRRFApproverBOL);
        }

        public void UpdateRRFForResend(RRFApproverBOL objRRFApproverBOL)
        {
            objRRFApproverDAL.UpdateRRFForResend(objRRFApproverBOL);
        }

        public DataSet GetEmployeeFromRole(RRFApproverBOL objRRFApproverBOL)
        {
            dsGetEmployeeFromRole = objRRFApproverDAL.GetEmployeeFromRole(objRRFApproverBOL);
            return dsGetEmployeeFromRole;
        }


        public DataSet GetSkillsForSLA(RRFApproverBOL objRRFApproverBOL)
        {
            dsSLAForTechnology = objRRFApproverDAL.GetSkillsForSLA(objRRFApproverBOL);
            return dsSLAForTechnology;
        }

        public DataSet GetSLADaysForSelectTechnology(int SkillForSLA)
        {
            dsTotalSLADaysForTech = objRRFApproverDAL.GetSLADaysForSelectTechnology(SkillForSLA);
            return dsTotalSLADaysForTech;
        }


        public DataSet getmaildetails(int RRFID)
        {
            dsmaildetails = objRRFApproverDAL.getmaildetails(RRFID);
            return dsmaildetails;
        }
    }
}