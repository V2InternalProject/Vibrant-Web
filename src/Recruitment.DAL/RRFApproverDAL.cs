using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BOL;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class RRFApproverDAL
    {
        DataSet dsGetRRFValuesToApprove = new DataSet();
        DataSet dsGetDeliveryUnit = new DataSet();
        DataSet dsGetDesignation = new DataSet();
        DataSet dsGetDeliveryTeam = new DataSet();
        DataSet dsGetResourcePool = new DataSet();
        DataSet dsGetEmployeeName = new DataSet();
        DataSet dsGetEmploymentType = new DataSet();
        DataSet dsAddRRF = new DataSet();
        DataSet dsGetEmployeeFromRole = new DataSet();
        DataSet dsTotalSLADaysForTech = new DataSet();
        DataSet dsmaildetails = new DataSet();

        public void UdateRRFValuesToApprove(RRFApproverBOL objRRFApproverBOL)
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[0].Value = objRRFApproverBOL.RRFID;

            param[1] = new SqlParameter("@Comments", SqlDbType.VarChar);
            param[1].Value = objRRFApproverBOL.Comments;

            param[2] = new SqlParameter("@RRFStatus", SqlDbType.Int);
            param[2].Value = objRRFApproverBOL.RRFStatus;

            param[3] = new SqlParameter("@ApprovalStatus", SqlDbType.Int);
            param[3].Value = objRRFApproverBOL.ApprovalStatus;

            param[4] = new SqlParameter("@ModifiedBy", SqlDbType.Int);
            param[4].Value = objRRFApproverBOL.ModifiedBy;

            param[5] = new SqlParameter("@ModifiedDate", SqlDbType.DateTime);
            param[5].Value = objRRFApproverBOL.ModifiedDate;

            SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_UdateRRFValuesToApprove", param);
        }

        public DataSet GetRRFValuesToApprove(RRFApproverBOL objRRFApproverBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RRFID", SqlDbType.VarChar);
            param[0].Value = objRRFApproverBOL.RRFID;
            return dsGetRRFValuesToApprove = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetRRFToApprove", param);
        }

        public DataSet GetDeliveryUnit(RRFApproverBOL objRRFApproverBOL)
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

        public DataSet GetDesignation(RRFApproverBOL objRRFApproverBOL)
        {
            return dsGetDesignation = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetDesignation");
        }

        public DataSet GetResourcePool(RRFApproverBOL objRRFApproverBOL)
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


        public DataSet GetEmployeeFromRole(RRFApproverBOL objRRFApproverBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Role", SqlDbType.VarChar);
            param[0].Value = objRRFApproverBOL.Role;
            return dsGetEmployeeFromRole = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetEmployeeFromRole", param);
        }


        public DataSet GetEmploymentType(RRFApproverBOL objRRFApproverBOL)
        {
            return dsGetEmploymentType = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetEmploymentType");
        }


        public void AddRRF(RRFApproverBOL objRRFApproverBOL)
        {
            SqlParameter[] param = new SqlParameter[28];

            param[0] = new SqlParameter("@RequestedBy", SqlDbType.Int);
            param[0].Value = objRRFApproverBOL.RequestedBy;

            param[1] = new SqlParameter("@RequestDate", SqlDbType.DateTime);
            param[1].Value = objRRFApproverBOL.RequestDate;

            param[2] = new SqlParameter("@ExpectedClosureDate", SqlDbType.DateTime);
            param[2].Value = objRRFApproverBOL.ExpectedClosureDate;

            param[3] = new SqlParameter("@RRFForDU", SqlDbType.Int);
            param[3].Value = objRRFApproverBOL.RRFForDU;

            param[4] = new SqlParameter("@RRFForDT", SqlDbType.Int);
            param[4].Value = objRRFApproverBOL.RRFForDT;

            param[5] = new SqlParameter("@ProjectName", SqlDbType.VarChar);
            param[5].Value = objRRFApproverBOL.ProjectName;

            param[6] = new SqlParameter("@Designation", SqlDbType.Int);
            param[6].Value = objRRFApproverBOL.Designation;

            param[7] = new SqlParameter("@IndicativePanel1", SqlDbType.Int);
            param[7].Value = objRRFApproverBOL.IndicativePanel1;

            param[8] = new SqlParameter("@IndicativePanel2", SqlDbType.Int);
            param[8].Value = objRRFApproverBOL.IndicativePanel2;

            param[9] = new SqlParameter("@IndicativePanel3", SqlDbType.Int);
            param[9].Value = objRRFApproverBOL.IndicativePanel3;

            param[10] = new SqlParameter("@ResourcePool", SqlDbType.Int);
            param[10].Value = objRRFApproverBOL.ResourcePool;

            param[11] = new SqlParameter("@PositionsRequired", SqlDbType.Int);
            param[11].Value = objRRFApproverBOL.PositionsRequired;

            param[12] = new SqlParameter("@EmployeementType", SqlDbType.Int);
            param[12].Value = objRRFApproverBOL.EmployeementType;

            param[13] = new SqlParameter("@IsReplacement", SqlDbType.Bit);
            param[13].Value = objRRFApproverBOL.IsReplacement;

            param[14] = new SqlParameter("@ReplacementFor", SqlDbType.Int);
            param[14].Value = objRRFApproverBOL.ReplacementFor;

            param[15] = new SqlParameter("@KeySkills", SqlDbType.VarChar);
            param[15].Value = objRRFApproverBOL.KeySkills;

            param[16] = new SqlParameter("@Experience", SqlDbType.Int);
            param[16].Value = objRRFApproverBOL.Experience;

            param[17] = new SqlParameter("@BusinessJustification", SqlDbType.VarChar);
            param[17].Value = objRRFApproverBOL.BusinessJustification;

            param[18] = new SqlParameter("@AdditionalInfo", SqlDbType.VarChar);
            param[18].Value = objRRFApproverBOL.AdditionalInfo;

            param[19] = new SqlParameter("@Budget", SqlDbType.Float);
            param[19].Value = objRRFApproverBOL.BudgetPerVacancy;

            param[20] = new SqlParameter("@ApprovedBy", SqlDbType.Int);
            param[20].Value = objRRFApproverBOL.ApprovedBy;

            param[21] = new SqlParameter("@ApproveDate", SqlDbType.DateTime);
            param[21].Value = objRRFApproverBOL.ApproveDate;

            param[22] = new SqlParameter("@RRFNo", SqlDbType.VarChar);
            param[22].Value = objRRFApproverBOL.RRFNo;

            param[23] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[23].Value = objRRFApproverBOL.RRFID;

            param[24] = new SqlParameter("@ModifiedBy", SqlDbType.Int);
            param[24].Value = objRRFApproverBOL.ApprovedBy;

            param[25] = new SqlParameter("@ModifiedDate", SqlDbType.DateTime);
            param[25].Value = objRRFApproverBOL.ApproveDate;

            param[26] = new SqlParameter("@IsBillable", SqlDbType.Bit);
            param[26].Value = objRRFApproverBOL.IsBillable;

            param[27] = new SqlParameter("@SLAForSkill", SqlDbType.Int);
            param[27].Value = objRRFApproverBOL.SLAForSkill;

            SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_AddRRFFromApprover", param);
        }

        public void UpdateRRFForResend(RRFApproverBOL objRRFApproverBOL)
        {
            SqlParameter[] param = new SqlParameter[21];

            param[0] = new SqlParameter("@RequestDate", SqlDbType.DateTime);
            param[0].Value = objRRFApproverBOL.RequestDate;

            param[1] = new SqlParameter("@ExpectedClosureDate", SqlDbType.DateTime);
            param[1].Value = objRRFApproverBOL.ExpectedClosureDate;

            param[2] = new SqlParameter("@RRFForDU", SqlDbType.Int);
            param[2].Value = objRRFApproverBOL.RRFForDU;

            param[3] = new SqlParameter("@RRFForDT", SqlDbType.Int);
            param[3].Value = objRRFApproverBOL.RRFForDT;

            param[4] = new SqlParameter("@ProjectName", SqlDbType.VarChar);
            param[4].Value = objRRFApproverBOL.ProjectName;

            param[5] = new SqlParameter("@Designation", SqlDbType.Int);
            param[5].Value = objRRFApproverBOL.Designation;

            param[6] = new SqlParameter("@IndicativePanel1", SqlDbType.Int);
            param[6].Value = objRRFApproverBOL.IndicativePanel1;

            param[7] = new SqlParameter("@IndicativePanel2", SqlDbType.Int);
            param[7].Value = objRRFApproverBOL.IndicativePanel2;

            param[8] = new SqlParameter("@IndicativePanel3", SqlDbType.Int);
            param[8].Value = objRRFApproverBOL.IndicativePanel3;

            param[9] = new SqlParameter("@ResourcePool", SqlDbType.Int);
            param[9].Value = objRRFApproverBOL.ResourcePool;

            param[10] = new SqlParameter("@PositionsRequired", SqlDbType.Int);
            param[10].Value = objRRFApproverBOL.PositionsRequired;

            param[11] = new SqlParameter("@EmployeementType", SqlDbType.Int);
            param[11].Value = objRRFApproverBOL.EmployeementType;

            param[12] = new SqlParameter("@IsReplacement", SqlDbType.Bit);
            param[12].Value = objRRFApproverBOL.IsReplacement;

            param[13] = new SqlParameter("@ReplacementFor", SqlDbType.Int);
            param[13].Value = objRRFApproverBOL.ReplacementFor;

            param[14] = new SqlParameter("@KeySkills", SqlDbType.VarChar);
            param[14].Value = objRRFApproverBOL.KeySkills;

            param[15] = new SqlParameter("@Experience", SqlDbType.Int);
            param[15].Value = objRRFApproverBOL.Experience;

            param[16] = new SqlParameter("@BusinessJustification", SqlDbType.VarChar);
            param[16].Value = objRRFApproverBOL.BusinessJustification;

            param[17] = new SqlParameter("@AdditionalInfo", SqlDbType.VarChar);
            param[17].Value = objRRFApproverBOL.AdditionalInfo;

            param[18] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[18].Value = objRRFApproverBOL.RRFID;

            param[19] = new SqlParameter("@ModifiedDate", SqlDbType.DateTime);
            param[19].Value = objRRFApproverBOL.RequestDate;

            param[20] = new SqlParameter("@IsBillable", SqlDbType.Bit);
            param[20].Value = objRRFApproverBOL.IsBillable;

            SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_UpdateRRFForResend", param);

        }

        public DataSet GetSkillsForSLA(RRFApproverBOL objRRFApproverBOL)
        {
            return dsGetDeliveryUnit = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_getSkillsForSLA");
        }

        public DataSet GetSLADaysForSelectTechnology(int SkillForSLA)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@TechnologyID", SqlDbType.Int);
            param[0].Value = SkillForSLA;

            return dsTotalSLADaysForTech = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "Sp_GetTotalDaysTechnology", param);
        }


        public DataSet getmaildetails(int RRFID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@RRFID", SqlDbType.Int);
            param[0].Value = RRFID;

            return dsmaildetails = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_getdetailsformail", param);
        }
    }
}