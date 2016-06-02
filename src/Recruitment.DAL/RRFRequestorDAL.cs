using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using BOL;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class RRFRequestorDAL
    {
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
            return dsGetApprover = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetApproverEmployeeList");
        }
        public DataSet GetDeliveryUnit()
        {
            return dsGetDeliveryUnit = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetDeliveryUnit");
        }

        public DataSet GetDeliveryTeam(int deliveryUnit)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@du", SqlDbType.Int);
            param[0].Value = deliveryUnit;
            return dsGetDeliveryTeam = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetDeliveryTeam", param);
        }

        public DataSet GetDesignation()
        {
            return dsGetDesignation = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetDesignation");
        }

        public DataSet GetResourcePool()
        {
            return dsGetResourcePool = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetResourcePool");
        }

        public DataSet GetEmployeeName(string prefixText)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@prefixText", SqlDbType.VarChar);
            param[0].Value = prefixText;
            return dsGetEmployeeName = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetEmployeeName", param);
        }


        public DataSet GetEmploymentType()
        {
            return dsGetEmploymentType = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetEmploymentType");
        }

        public DataSet AddRRF(RRFRequestorBOL objRRFRequestorBOL)
        {
            SqlParameter[] param = new SqlParameter[25];

            param[0] = new SqlParameter("@RequestedBy", SqlDbType.Int);
            param[0].Value = objRRFRequestorBOL.RequestedBy;

            param[1] = new SqlParameter("@RequestDate", SqlDbType.DateTime);
            param[1].Value = objRRFRequestorBOL.RequestDate;

            param[2] = new SqlParameter("@ExpectedClosureDate", SqlDbType.DateTime);
            param[2].Value = objRRFRequestorBOL.ExpectedClosureDate;

            param[3] = new SqlParameter("@RRFForDU", SqlDbType.Int);
            param[3].Value = objRRFRequestorBOL.RRFForDU;

            param[4] = new SqlParameter("@RRFForDT", SqlDbType.Int);
            if (objRRFRequestorBOL.RRFForDT == 0)
                param[4].Value = DBNull.Value;
            else
            {
                param[4].Value = objRRFRequestorBOL.RRFForDT;
            }

            param[5] = new SqlParameter("@ProjectName", SqlDbType.VarChar);
            if (objRRFRequestorBOL.ProjectName == "")
                param[5].Value = DBNull.Value;
            else
                param[5].Value = objRRFRequestorBOL.ProjectName;

            param[6] = new SqlParameter("@Designation", SqlDbType.Int);
            if (objRRFRequestorBOL.Designation == 0)
                param[6].Value = DBNull.Value;
            else
            {
                param[6].Value = objRRFRequestorBOL.Designation;
            }

            param[7] = new SqlParameter("@IndicativePanel1", SqlDbType.Int);
            if (objRRFRequestorBOL.IndicativePanel1 == 0)
                param[7].Value = DBNull.Value;
            else
            {
                param[7].Value = objRRFRequestorBOL.IndicativePanel1;
            }

            param[8] = new SqlParameter("@IndicativePanel2", SqlDbType.Int);
            if (objRRFRequestorBOL.IndicativePanel2 == 0)
                param[8].Value = DBNull.Value;
            else
            {
                param[8].Value = objRRFRequestorBOL.IndicativePanel2;
            }

            param[9] = new SqlParameter("@IndicativePanel3", SqlDbType.Int);
            if (objRRFRequestorBOL.IndicativePanel3 == 0)
                param[9].Value = DBNull.Value;
            else
            {
                param[9].Value = objRRFRequestorBOL.IndicativePanel3;
            }

            param[10] = new SqlParameter("@ResourcePool", SqlDbType.Int);
            if (objRRFRequestorBOL.ResourcePool == 0)
                param[10].Value = DBNull.Value;
            else
            {
                param[10].Value = objRRFRequestorBOL.ResourcePool;
            }

            param[11] = new SqlParameter("@PositionsRequired", SqlDbType.Int);
            param[11].Value = objRRFRequestorBOL.PositionsRequired;

            param[12] = new SqlParameter("@EmployeementType", SqlDbType.Int);
            if (objRRFRequestorBOL.EmployeementType == 0)
                param[12].Value = DBNull.Value;
            else
            {
                param[12].Value = objRRFRequestorBOL.EmployeementType;
            }

            param[13] = new SqlParameter("@IsReplacement", SqlDbType.Bit);
            param[13].Value = objRRFRequestorBOL.IsReplacement;

            param[14] = new SqlParameter("@ReplacementFor", SqlDbType.Int);
            if (objRRFRequestorBOL.ReplacementFor == 0)
                param[14].Value = DBNull.Value;
            else
            {
                param[14].Value = objRRFRequestorBOL.ReplacementFor;
            }

            param[15] = new SqlParameter("@KeySkills", SqlDbType.VarChar);
            param[15].Value = objRRFRequestorBOL.KeySkills;

            param[16] = new SqlParameter("@Experience", SqlDbType.Int);
            param[16].Value = objRRFRequestorBOL.Experience;

            param[17] = new SqlParameter("@BusinessJustification", SqlDbType.VarChar);
            if (objRRFRequestorBOL.BusinessJustification == "")
                param[17].Value = DBNull.Value;
            else
                param[17].Value = objRRFRequestorBOL.BusinessJustification;

            param[18] = new SqlParameter("@AdditionalInfo", SqlDbType.VarChar);
            if (objRRFRequestorBOL.AdditionalInfo == "")
                param[18].Value = DBNull.Value;
            else
                param[18].Value = objRRFRequestorBOL.AdditionalInfo;

            param[19] = new SqlParameter("@DUName", SqlDbType.VarChar);
            param[19].Value = objRRFRequestorBOL.DUName;

            param[20] = new SqlParameter("@ApprovedBy", SqlDbType.Int);
            param[20].Value = objRRFRequestorBOL.ApprovedBy;

            param[21] = new SqlParameter("@ModifiedBy", SqlDbType.Int);
            param[21].Value = objRRFRequestorBOL.RequestedBy;

            param[22] = new SqlParameter("@ModifiedDate", SqlDbType.DateTime);
            param[22].Value = objRRFRequestorBOL.RequestDate;

            param[23] = new SqlParameter("@IsBillable", SqlDbType.Bit);
            param[23].Value = objRRFRequestorBOL.IsBillable;

            param[24] = new SqlParameter("@SLAForSkill", SqlDbType.Int);
            param[24].Value = objRRFRequestorBOL.SLAForSkill;

            return dsAddRRF = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_AddtblRRF", param);
        }

        public DataSet GetSkillsForSLA()
        {
            return dsGetEmploymentType = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_getSkillsForSLA");
        }

        public DataSet GetSLADaysForSelectTechnology(int SkillForSLA)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@TechnologyID", SqlDbType.Int);
            param[0].Value = SkillForSLA;

            return dsTotalSLADaysForTech = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "Sp_GetTotalDaysTechnology", param);
        }
    }
}
