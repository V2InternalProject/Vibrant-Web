using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using DAL;
using BOL;

namespace BLL
{
    public class RRFRequestorBLL
    {
        RRFRequestorDAL objRRFRequestorDAL = new RRFRequestorDAL();
        DataSet dsGetDeliveryUnit = new DataSet();
        DataSet dsGetDesignation = new DataSet();
        DataSet dsGetDeliveryTeam = new DataSet();
        DataSet dsGetResourcePool = new DataSet();
        DataSet dsGetEmployeeName = new DataSet();
        DataSet dsGetEmploymentType = new DataSet();
        DataSet dsAddRRF = new DataSet();
        DataSet dsGetApprover = new DataSet();
        DataSet dsSLAForTechnology = new DataSet();
        DataSet dsTotalSLADaysForTech = new DataSet();

        public DataSet GetApprover()
        {
            dsGetApprover = objRRFRequestorDAL.GetApprover();
            return dsGetApprover;
        }
        public DataSet GetDeliveryUnit()
        {
            dsGetDeliveryUnit = objRRFRequestorDAL.GetDeliveryUnit();
            return dsGetDeliveryUnit;
        }

        public DataSet GetDeliveryTeam(int deliveryUnit)
        {
            dsGetDeliveryTeam = objRRFRequestorDAL.GetDeliveryTeam(deliveryUnit);
            return dsGetDeliveryTeam;
        }

        public DataSet GetDesignation()
        {
            dsGetDesignation = objRRFRequestorDAL.GetDesignation();
            return dsGetDesignation;
        }

        public DataSet GetResourcePool()
        {
            dsGetResourcePool = objRRFRequestorDAL.GetResourcePool();
            return dsGetResourcePool;
        }

        public DataSet GetEmployeeName(string prefixText)
        {
            dsGetEmployeeName = objRRFRequestorDAL.GetEmployeeName(prefixText);
            return dsGetEmployeeName;
        }

        public DataSet GetEmploymentType()
        {
            dsGetEmploymentType = objRRFRequestorDAL.GetEmploymentType();
            return dsGetEmploymentType;
        }

        public DataSet AddRRF(RRFRequestorBOL objRRFRequestorBOL)
        {
            dsAddRRF = objRRFRequestorDAL.AddRRF(objRRFRequestorBOL);
            return dsAddRRF;
        }

        public DataSet GetSkillsForSLA()
        {
            dsSLAForTechnology = objRRFRequestorDAL.GetSkillsForSLA();
            return dsSLAForTechnology;
        }

        public DataSet GetSLADaysForSelectTechnology(int SkillForSLA)
        {
            dsTotalSLADaysForTech = objRRFRequestorDAL.GetSLADaysForSelectTechnology(SkillForSLA);
            return dsTotalSLADaysForTech;
        }
    }
}
