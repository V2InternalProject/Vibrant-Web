using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BOL;
using System.Data.SqlClient;

namespace DAL
{
    public class CandidateDAL
    {
        DataSet dsCandidateDAL = new DataSet();

        public DataSet AddCandidate(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[40];

            param[0] = new SqlParameter("@CandidateStatus", SqlDbType.Int);
            param[0].Value = candidateBOL.CandidateStatus;

            param[1] = new SqlParameter("@Salutation", SqlDbType.VarChar);
            param[1].Value = candidateBOL.Salutation;

            param[2] = new SqlParameter("@FirstName", SqlDbType.VarChar);
            param[2].Value = candidateBOL.FirstName;

            param[3] = new SqlParameter("@Middlename", SqlDbType.VarChar);
            param[3].Value = candidateBOL.Middlename;

            param[4] = new SqlParameter("@LastName", SqlDbType.VarChar);
            param[4].Value = candidateBOL.LastName;

            param[5] = new SqlParameter("@DateOfBirth", SqlDbType.DateTime);
            param[5].Value = candidateBOL.DateOfBirth;

            param[6] = new SqlParameter("@MaritalStatus", SqlDbType.VarChar);
            param[6].Value = candidateBOL.MaritalStatus;

            param[7] = new SqlParameter("@Gender", SqlDbType.VarChar);
            param[7].Value = candidateBOL.Gender;

            param[8] = new SqlParameter("@AlternateContactNumber", SqlDbType.VarChar);
            param[8].Value = candidateBOL.AlternateContactNumber;

            param[9] = new SqlParameter("@MobileNumber", SqlDbType.VarChar);
            param[9].Value = candidateBOL.MobileNumber;

            param[10] = new SqlParameter("@TotalWorkExperienceInYear", SqlDbType.Int);
            param[10].Value = candidateBOL.TotalWorkExperienceInYear;

            param[11] = new SqlParameter("@TotalWorkExperienceInMonths", SqlDbType.Int);
            param[11].Value = candidateBOL.TotalWorkExperienceInMonths;

            param[12] = new SqlParameter("@RelevantWorkExperienceInYear", SqlDbType.Int);
            param[12].Value = candidateBOL.RelevantWorkExperienceInYear;

            param[13] = new SqlParameter("@RelevantWorkExperienceInMonths", SqlDbType.Int);
            param[13].Value = candidateBOL.RelevantWorkExperienceInMonths;

            param[14] = new SqlParameter("@WorkExperienceInMonths", SqlDbType.Int);
            param[14].Value = candidateBOL.WorkExperienceInMonths;

            param[15] = new SqlParameter("@RelevantlWorkExperienceInMonths", SqlDbType.Int);
            param[15].Value = candidateBOL.RelevantlWorkExperienceInMonths;

            param[16] = new SqlParameter("@HighestQualification", SqlDbType.Int);
            param[16].Value = candidateBOL.HighestQualification;

            param[17] = new SqlParameter("@ValidPassport", SqlDbType.Bit);
            param[17].Value = candidateBOL.ValidPassPort;


            param[18] = new SqlParameter("@USVisa", SqlDbType.Bit);
            param[18].Value = candidateBOL.USVisa;

            param[19] = new SqlParameter("@WillingToRelocate", SqlDbType.Bit);
            param[19].Value = candidateBOL.WillingToRelocate;

            param[20] = new SqlParameter("@CurrentAddress", SqlDbType.VarChar);
            param[20].Value = candidateBOL.PresentAddress;

            param[21] = new SqlParameter("@State", SqlDbType.VarChar);
            param[21].Value = candidateBOL.State;

            param[22] = new SqlParameter("@City", SqlDbType.VarChar);
            param[22].Value = candidateBOL.City;

            param[23] = new SqlParameter("@ZipCode", SqlDbType.VarChar);
            param[23].Value = candidateBOL.PinCode;

            param[24] = new SqlParameter("@CurrentCTC", SqlDbType.Decimal);
            param[24].Value = candidateBOL.CurrentCTC;

            param[25] = new SqlParameter("@AreaOfInterest", SqlDbType.VarChar);
            param[25].Value = candidateBOL.AreasOfInterest;

            param[26] = new SqlParameter("@Source", SqlDbType.VarChar);
            param[26].Value = candidateBOL.Source;


            param[27] = new SqlParameter("@SourceName", SqlDbType.VarChar);
            param[27].Value = candidateBOL.SourceName;


            param[28] = new SqlParameter("@CurrentJobSummary", SqlDbType.VarChar);
            param[28].Value = candidateBOL.CurrentJobSummary;


            param[29] = new SqlParameter("@Recognition", SqlDbType.VarChar);
            param[29].Value = candidateBOL.RewardsAndRecognition;


            param[30] = new SqlParameter("@Achievements", SqlDbType.VarChar);
            param[30].Value = candidateBOL.SpecialAchievements;


            param[31] = new SqlParameter("@OtherSkills", SqlDbType.VarChar);
            param[31].Value = candidateBOL.OtherSkills;

            param[32] = new SqlParameter("@NoticePeriod", SqlDbType.Int);
            param[32].Value = candidateBOL.CurrrentNoticePeriod;


            param[33] = new SqlParameter("@CurrentRRF", SqlDbType.Int);
            param[33].Value = candidateBOL.CurrentRRF;

            param[34] = new SqlParameter("@CreatedBy", SqlDbType.Int);
            param[34].Value = candidateBOL.CreatedBy;

            param[35] = new SqlParameter("@CreatedDate", SqlDbType.DateTime);
            param[35].Value = candidateBOL.CreatedDate;

            param[36] = new SqlParameter("@Email", SqlDbType.VarChar);
            param[36].Value = candidateBOL.Email;

            param[37] = new SqlParameter("@IsFileUploaded", SqlDbType.Bit);
            param[37].Value = candidateBOL.IsFileUploaded;

            param[38] = new SqlParameter("@AlternateEmailID", SqlDbType.VarChar);
            param[38].Value = candidateBOL.AlternateEmailID;

            param[39] = new SqlParameter("@Country", SqlDbType.Int);
            param[39].Value = candidateBOL.Country;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_AddCandidate_ChangeForCountry", param);

        }

        public DataSet UpdateCandidate(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[41];

            param[0] = new SqlParameter("@CandidateStatus", SqlDbType.Int);
            param[0].Value = candidateBOL.CandidateStatus;

            param[1] = new SqlParameter("@Salutation", SqlDbType.VarChar);
            param[1].Value = candidateBOL.Salutation;

            param[2] = new SqlParameter("@FirstName", SqlDbType.VarChar);
            param[2].Value = candidateBOL.FirstName;

            param[3] = new SqlParameter("@Middlename", SqlDbType.VarChar);
            param[3].Value = candidateBOL.Middlename;

            param[4] = new SqlParameter("@LastName", SqlDbType.VarChar);
            param[4].Value = candidateBOL.LastName;

            param[5] = new SqlParameter("@DateOfBirth", SqlDbType.DateTime);
            param[5].Value = candidateBOL.DateOfBirth;

            param[6] = new SqlParameter("@MaritalStatus", SqlDbType.VarChar);
            param[6].Value = candidateBOL.MaritalStatus;

            param[7] = new SqlParameter("@Gender", SqlDbType.VarChar);
            param[7].Value = candidateBOL.Gender;

            param[8] = new SqlParameter("@AlternateContactNumber", SqlDbType.VarChar);
            param[8].Value = candidateBOL.AlternateContactNumber;

            param[9] = new SqlParameter("@MobileNumber", SqlDbType.VarChar);
            param[9].Value = candidateBOL.MobileNumber;

            param[10] = new SqlParameter("@TotalWorkExperienceInYear", SqlDbType.Int);
            param[10].Value = candidateBOL.TotalWorkExperienceInYear;

            param[11] = new SqlParameter("@TotalWorkExperienceInMonths", SqlDbType.Int);
            param[11].Value = candidateBOL.TotalWorkExperienceInMonths;

            param[12] = new SqlParameter("@RelevantWorkExperienceInYear", SqlDbType.Int);
            param[12].Value = candidateBOL.RelevantWorkExperienceInYear;

            param[13] = new SqlParameter("@RelevantWorkExperienceInMonths", SqlDbType.Int);
            param[13].Value = candidateBOL.RelevantWorkExperienceInMonths;

            param[14] = new SqlParameter("@WorkExperienceInMonths", SqlDbType.Int);
            param[14].Value = candidateBOL.WorkExperienceInMonths;

            param[15] = new SqlParameter("@RelevantlWorkExperienceInMonths", SqlDbType.Int);
            param[15].Value = candidateBOL.RelevantlWorkExperienceInMonths;

            param[16] = new SqlParameter("@HighestQualification", SqlDbType.Int);
            param[16].Value = candidateBOL.HighestQualification;

            param[17] = new SqlParameter("@ValidPassport", SqlDbType.Bit);
            param[17].Value = candidateBOL.ValidPassPort;


            param[18] = new SqlParameter("@USVisa", SqlDbType.Bit);
            param[18].Value = candidateBOL.USVisa;

            param[19] = new SqlParameter("@WillingToRelocate", SqlDbType.Bit);
            param[19].Value = candidateBOL.WillingToRelocate;

            param[20] = new SqlParameter("@CurrentAddress", SqlDbType.VarChar);
            param[20].Value = candidateBOL.PresentAddress;

            param[21] = new SqlParameter("@State", SqlDbType.VarChar);
            param[21].Value = candidateBOL.State;

            param[22] = new SqlParameter("@City", SqlDbType.VarChar);
            param[22].Value = candidateBOL.City;

            param[23] = new SqlParameter("@ZipCode", SqlDbType.VarChar);
            param[23].Value = candidateBOL.PinCode;

            param[24] = new SqlParameter("@CurrentCTC", SqlDbType.Decimal);
            param[24].Value = candidateBOL.CurrentCTC;

            param[25] = new SqlParameter("@AreaOfInterest", SqlDbType.VarChar);
            param[25].Value = candidateBOL.AreasOfInterest;

            param[26] = new SqlParameter("@Source", SqlDbType.VarChar);
            param[26].Value = candidateBOL.Source;


            param[27] = new SqlParameter("@SourceName", SqlDbType.VarChar);
            param[27].Value = candidateBOL.SourceName;


            param[28] = new SqlParameter("@CurrentJobSummary", SqlDbType.VarChar);
            param[28].Value = candidateBOL.CurrentJobSummary;


            param[29] = new SqlParameter("@Recognition", SqlDbType.VarChar);
            param[29].Value = candidateBOL.RewardsAndRecognition;


            param[30] = new SqlParameter("@Achievements", SqlDbType.VarChar);
            param[30].Value = candidateBOL.SpecialAchievements;


            param[31] = new SqlParameter("@OtherSkills", SqlDbType.VarChar);
            param[31].Value = candidateBOL.OtherSkills;

            param[32] = new SqlParameter("@NoticePeriod", SqlDbType.Int);
            param[32].Value = candidateBOL.CurrrentNoticePeriod;


            param[33] = new SqlParameter("@CreatedBy", SqlDbType.Int);
            param[33].Value = candidateBOL.CreatedBy;

            param[34] = new SqlParameter("@CreatedDate", SqlDbType.DateTime);
            param[34].Value = candidateBOL.CreatedDate;

            param[35] = new SqlParameter("@Email", SqlDbType.VarChar);
            param[35].Value = candidateBOL.Email;

            param[36] = new SqlParameter("@ID", SqlDbType.BigInt);
            param[36].Value = candidateBOL.CandidateID;

            param[37] = new SqlParameter("@IsFileUploaded", SqlDbType.Bit);
            param[37].Value = candidateBOL.IsFileUploaded;

            param[38] = new SqlParameter("@AlternateEmailID", SqlDbType.VarChar);
            param[38].Value = candidateBOL.AlternateEmailID;

            param[39] = new SqlParameter("@JoiningDate", SqlDbType.DateTime);
            param[39].Value = candidateBOL.JoiningDate;

            param[40] = new SqlParameter("@Country", SqlDbType.Int);
            param[40].Value = candidateBOL.Country;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_UpdateCandidate_ChangeForCountry", param);

        }


        public DataSet UpdateCandidateFileUploadStatus(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@ID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;

            param[1] = new SqlParameter("@IsFileUploaded", SqlDbType.Bit);
            param[1].Value = candidateBOL.IsFileUploaded;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_UpdateCandidateFileUpload", param);

        }

        public DataSet GetCandidateDetails(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[1];


            param[0] = new SqlParameter("@ID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateDetails_ChangeForCountry", param);

        }
        public DataSet GetCandidateHistory(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[1];


            param[0] = new SqlParameter("@ID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "GetCandidateHistory", param);

        }
        public DataSet GetCandidateDetailsByFirstNameAndLastNameAndDOB(string firstName, string lastname, string DOB)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@FirstName", SqlDbType.VarChar);
            param[0].Value = firstName;

            param[1] = new SqlParameter("@LastName", SqlDbType.VarChar);
            param[1].Value = lastname;

            param[2] = new SqlParameter("@DateOfBirth", SqlDbType.DateTime);
            param[2].Value = DateTime.ParseExact(DOB, "MM/dd/yyyy", null);

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateDetailsByFirstNameAndLastNameAndDOB", param);

        }



        public DataSet AddCandidateSkills(CandidateBOL candidateBOL)
        {

            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@SkillID", SqlDbType.Int);
            param[0].Value = candidateBOL.SkillID;

            param[1] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[1].Value = candidateBOL.CandidateID;

            param[2] = new SqlParameter("@LastModifiedBy", SqlDbType.Int);
            param[2].Value = candidateBOL.CreatedBy;

            param[3] = new SqlParameter("@LastModifiedDate", SqlDbType.DateTime);
            param[3].Value = candidateBOL.CreatedDate;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_AddCandidateSkills", param);

        }

        public DataSet UpdateCandidateSkills(CandidateBOL candidateBOL)
        {

            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@SkillID", SqlDbType.Int);
            param[0].Value = candidateBOL.SkillID;

            param[1] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[1].Value = candidateBOL.CandidateID;

            param[2] = new SqlParameter("@LastModifiedBy", SqlDbType.Int);
            param[2].Value = candidateBOL.CreatedBy;

            param[3] = new SqlParameter("@LastModifiedDate", SqlDbType.DateTime);
            param[3].Value = candidateBOL.CreatedDate;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_UpdateCandidateSkills", param);

        }


        public string GetLatestExperienceID(CandidateBOL candidateBOL)
        {
            dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetLatestExperienceID");
            return Convert.ToString(dsCandidateDAL.Tables[0].Rows[0][0]);
        }

        public string GetLatestCertificationID(CandidateBOL candidateBOL)
        {
            dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetLatestCertificationID");
            return Convert.ToString(dsCandidateDAL.Tables[0].Rows[0][0]);
        }

        public string GetLatestEducationID(CandidateBOL candidateBOL)
        {
            dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetLatestEducationID");
            return Convert.ToString(dsCandidateDAL.Tables[0].Rows[0][0]);
        }


        public DataSet GetCandidateExpID(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateExpID_ChangeForType", param);
        }

        public DataSet GetCandidateCertificationID(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateCertificationID", param);
        }

        public DataSet GetCandidateEducationID(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateEducationID_ChangeForDegree", param);
        }



        /// <summary>
        /// Gets Experience Details by CandidateID if there is no Experience Details for a Candidate
        /// </summary>
        /// <param name="candidateBOL"></param>
        /// <returns></returns>
        public DataSet GetExperienceDetails(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateExperienceDetails_ChangeForType", param);
        }

        /// <summary>
        /// Gets Experience Details by candidateID
        /// </summary>
        /// <param name="candidateID"></param>
        /// <returns></returns>
        public DataSet GetExperienceDetails(string candidateID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateID;
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateExperienceDetails_ChangeForType", param);
        }

        public DataSet GetMaxjoinedcandidate(string candidateID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateID;
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "GetMaxjoinedcandidate", param);
        }

        public DataSet GetEducationDetails(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateEducationDetails_ChangeForDegree", param);

        }

        public DataSet GetEducationDetails(string candidateID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateID;
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateEducationDetails_ChangeForDegree", param);

        }



        public DataSet GetCertificationDetails(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateCertificationDetails", param);

        }

        public DataSet GetCertificationDetails(string candidateID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateID;
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateCertificationDetails", param);

        }


        public DataSet GetAllCourses()
        {
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateQualification_Courses");
        }

        public DataSet GetAllCountryNames()
        {
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateCountryNames");
        }

        public DataSet GetAllCertificationNames()
        {
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateCertificationNames");
        }

        public DataSet GetAllDegree()
        {
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateQualificationDegree");
        }

        public DataSet GetAllCourseTypes()
        {
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateQualificationType_WHCMDEMO");
        }

        public DataSet GetAllExpTypes()
        {
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateExperienceType_WHCMDEMO");
        }

        /// <summary>
        ///  Gets Establishment list
        /// </summary>
        /// <returns></returns>
        //public DataSet GetAllEstablishment()
        //{
        //    return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetEstablishments");
        //}
        /// <summary>
        /// Gets Organization list
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllOrganization()
        {
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetOrganizations");
        }

        public DataSet GetAllSkills(string mode)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Mode", SqlDbType.VarChar);
            param[0].Value = mode;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateSkills", param);
        }

        public DataSet GetCandidateSkills(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateSkillDetails", param);
        }

        public DataSet GetAllStaus()
        {
            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateStatus");
        }

        //public DataSet GetAllCoursesByPGUG(string PGUG)
        //{
        //    SqlParameter[] param = new SqlParameter[1];
        //    param[0] = new SqlParameter("@PGUG", SqlDbType.VarChar);
        //    param[0].Value = PGUG;
        //    return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_GetCandidateQualificationByPGUG", param);
        //}

        public DataSet AddCandidateExperienceDetails(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[10];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;

            param[1] = new SqlParameter("@OrganisationName", SqlDbType.NVarChar);
            param[1].Value = candidateBOL.OrganisationName;

            //param[2] = new SqlParameter("@Location", SqlDbType.VarChar);
            //param[2].Value = candidateBOL.Location;


            param[2] = new SqlParameter("@WorkedFrom", SqlDbType.DateTime);
            param[2].Value = candidateBOL.WorkedFrom;

            param[3] = new SqlParameter("@WorkedTill", SqlDbType.DateTime);
            if (candidateBOL.WorkedTill == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                param[3].Value = null;
            else
                param[3].Value = candidateBOL.WorkedTill;


            param[4] = new SqlParameter("@PositionHeld", SqlDbType.VarChar);
            param[4].Value = candidateBOL.PositionHeld;

            param[5] = new SqlParameter("@CTC", SqlDbType.VarChar);
            param[5].Value = candidateBOL.CTC;

            param[6] = new SqlParameter("@LastModifiedBy", SqlDbType.Int);
            param[6].Value = candidateBOL.LastModifiedBy;

            param[7] = new SqlParameter("@LastModifiedDate", SqlDbType.DateTime);
            param[7].Value = candidateBOL.LastModifiedDate;

            param[8] = new SqlParameter("@ExpType", SqlDbType.Int);
            param[8].Value = candidateBOL.ExpType;

            param[9] = new SqlParameter("@ReportingManager", SqlDbType.VarChar);
            param[9].Value = candidateBOL.ReportingManager;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_AddCandidateExperienceDetails_ChangeForType", param);

        }

        public DataSet UpdateCandidateExperienceDetails(int ID, CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[10];

            param[0] = new SqlParameter("@ExpID", SqlDbType.Int);
            param[0].Value = ID;

            param[1] = new SqlParameter("@OrganisationName", SqlDbType.NVarChar);
            param[1].Value = candidateBOL.OrganisationName;

            //param[2] = new SqlParameter("@Location", SqlDbType.VarChar);
            //param[2].Value = candidateBOL.Location;


            param[2] = new SqlParameter("@WorkedFrom", SqlDbType.DateTime);
            param[2].Value = candidateBOL.WorkedFrom;

            param[3] = new SqlParameter("@WorkedTill", SqlDbType.DateTime);
            if (candidateBOL.WorkedTill == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                param[3].Value = null;
            else
                param[3].Value = candidateBOL.WorkedTill; ;

            param[4] = new SqlParameter("@PositionHeld", SqlDbType.VarChar);
            param[4].Value = candidateBOL.PositionHeld;

            param[5] = new SqlParameter("@CTC", SqlDbType.VarChar);
            param[5].Value = candidateBOL.CTC;

            param[6] = new SqlParameter("@LastModifiedBy", SqlDbType.Int);
            param[6].Value = candidateBOL.LastModifiedBy;

            param[7] = new SqlParameter("@LastModifiedDate", SqlDbType.DateTime);
            param[7].Value = candidateBOL.LastModifiedDate;

            param[8] = new SqlParameter("@ExpType", SqlDbType.Int);
            param[8].Value = candidateBOL.ExpType;

            param[9] = new SqlParameter("@ReportingManager", SqlDbType.VarChar);
            param[9].Value = candidateBOL.ReportingManager;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_UpdateCandidateExperienceDetails_ChangeForType", param);

        }


        public DataSet AddCandidateCertificationDetails(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[7];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;

            param[1] = new SqlParameter("@CertificationName", SqlDbType.VarChar);
            param[1].Value = candidateBOL.CertificationName;

            param[2] = new SqlParameter("@CertificationNo", SqlDbType.VarChar);
            param[2].Value = candidateBOL.CertificationNo;

            param[3] = new SqlParameter("@Institution", SqlDbType.VarChar);
            param[3].Value = candidateBOL.Institution;

            param[4] = new SqlParameter("@CertificationDate", SqlDbType.DateTime);
            param[4].Value = candidateBOL.CertificationDate;

            param[5] = new SqlParameter("@CertificationScore", SqlDbType.VarChar);
            param[5].Value = candidateBOL.CertificationScore;

            param[6] = new SqlParameter("@CertificationGrade", SqlDbType.VarChar);
            param[6].Value = candidateBOL.CertificationGrade;


            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_AddCandidateCertificationDetails", param);

        }


        public DataSet UpdateCandidateCertificationDetails(int ID, CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[7];

            param[0] = new SqlParameter("@CertificationID", SqlDbType.Int);
            param[0].Value = ID;

            param[1] = new SqlParameter("@CertificationName", SqlDbType.VarChar);
            param[1].Value = candidateBOL.CertificationName;

            param[2] = new SqlParameter("@CertificationNo", SqlDbType.VarChar);
            param[2].Value = candidateBOL.CertificationNo;

            param[3] = new SqlParameter("@Institution", SqlDbType.VarChar);
            param[3].Value = candidateBOL.Institution;

            param[4] = new SqlParameter("@CertificationDate", SqlDbType.DateTime);
            param[4].Value = candidateBOL.CertificationDate;

            param[5] = new SqlParameter("@CertificationScore", SqlDbType.VarChar);
            param[5].Value = candidateBOL.CertificationScore;

            param[6] = new SqlParameter("@CertificationGrade", SqlDbType.VarChar);
            param[6].Value = candidateBOL.CertificationGrade;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_UpdateCandidateCertificationDetails", param);

        }

        public DataSet AddCandidateEducationDetails(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[8];

            param[0] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[0].Value = candidateBOL.CandidateID;

            //param[1] = new SqlParameter("@PGUG", SqlDbType.VarChar);
            //param[1].Value = candidateBOL.PGUG;

            param[1] = new SqlParameter("@Degree", SqlDbType.VarChar);
            param[1].Value = candidateBOL.Degree;

            param[2] = new SqlParameter("@Specialization", SqlDbType.VarChar);
            param[2].Value = candidateBOL.Specialization;

            //param[3] = new SqlParameter("@Institute", SqlDbType.VarChar);
            //param[3].Value = candidateBOL.Institute;

            param[3] = new SqlParameter("@University", SqlDbType.VarChar);
            param[3].Value = candidateBOL.University;

            param[4] = new SqlParameter("@Course", SqlDbType.Int);
            param[4].Value = candidateBOL.Course;

            param[5] = new SqlParameter("@Year", SqlDbType.Int);
            param[5].Value = candidateBOL.Year;

            param[6] = new SqlParameter("@Type", SqlDbType.Int);
            param[6].Value = candidateBOL.Type;

            param[7] = new SqlParameter("@Percentage", SqlDbType.VarChar);
            param[7].Value = candidateBOL.Percentage;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_AddCandidateEducationDetails_ChangeForDegree", param);

        }


        public DataSet UpdateCandidateEducationDetails(int ID, CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[8];

            param[0] = new SqlParameter("@EducationID", SqlDbType.Int);
            param[0].Value = ID;

            //param[1] = new SqlParameter("@PGUG", SqlDbType.VarChar);
            //param[1].Value = candidateBOL.PGUG;

            param[1] = new SqlParameter("@Degree", SqlDbType.VarChar);
            param[1].Value = candidateBOL.Degree;

            param[2] = new SqlParameter("@Specialization", SqlDbType.VarChar);
            param[2].Value = candidateBOL.Specialization;

            //param[3] = new SqlParameter("@Institute", SqlDbType.VarChar);
            //param[3].Value = candidateBOL.Institute;

            param[3] = new SqlParameter("@University", SqlDbType.VarChar);
            param[3].Value = candidateBOL.University;

            param[4] = new SqlParameter("@Course", SqlDbType.Int);
            param[4].Value = candidateBOL.Course;

            param[5] = new SqlParameter("@Year", SqlDbType.Int);
            param[5].Value = candidateBOL.Year;

            param[6] = new SqlParameter("@Type", SqlDbType.Int);
            param[6].Value = candidateBOL.Type;

            param[7] = new SqlParameter("@Percentage", SqlDbType.VarChar);
            param[7].Value = candidateBOL.Percentage;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_UpdateCandidateEducationDetails_ChangeForDegree", param);

        }

        public DataSet DeleteCandidateExperienceDetails(int ID)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@ExpID", SqlDbType.Int);
            param[0].Value = ID;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_DeleteCandidateExperienceDetails_ChangeForType", param);
        }

        public DataSet DeleteCandidateCertificationDetails(int ID)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@CertificationID", SqlDbType.Int);
            param[0].Value = ID;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_DeleteCandidateCertificationDetails", param);
        }

        public DataSet DeleteCandidateEducationDetails(int ID)
        {
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@EducationID", SqlDbType.Int);
            param[0].Value = ID;

            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_DeleteCandidateEducationDetails_ChangeForDegree", param);
        }


        public DataSet DeleteCandidateSkills(CandidateBOL candidateBOL)
        {
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@SkillID", SqlDbType.Int);
            param[0].Value = candidateBOL.SkillID;

            param[1] = new SqlParameter("@CandidateID", SqlDbType.BigInt);
            param[1].Value = candidateBOL.CandidateID;


            return dsCandidateDAL = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString, CommandType.StoredProcedure, "sp_DeleteCandidateSkills", param);

        }




    }
}
