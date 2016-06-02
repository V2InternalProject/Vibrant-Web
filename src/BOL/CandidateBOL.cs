using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOL
{
    [Serializable]
    public class CandidateBOL
    {
        # region Candidate Properties

        public int CandidateID { get; set; }
        public int CandidateStatus { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string Middlename { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public string AlternateContactNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool IsFileUploaded { get; set; }
        public string AlternateEmailID { get; set; }

        public int TotalWorkExperienceInYear { get; set; }
        public int TotalWorkExperienceInMonths { get; set; }
        public int RelevantWorkExperienceInYear { get; set; }
        public int RelevantWorkExperienceInMonths { get; set; }
        public int WorkExperienceInMonths { get; set; }
        public int RelevantlWorkExperienceInMonths { get; set; }

        public int HighestQualification { get; set; }
        public bool ValidPassPort { get; set; }
        public bool USVisa { get; set; }
        public bool WillingToRelocate { get; set; }
        public string PresentAddress { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int Country { get; set; }
        public string PinCode { get; set; }
        public int CurrrentNoticePeriod { get; set; }
        public decimal CurrentCTC { get; set; }
        public string AreasOfInterest { get; set; }
        public string Source { get; set; }
        public string SourceName { get; set; }
        public string CurrentJobSummary { get; set; }
        public string RewardsAndRecognition { get; set; }
        public string SpecialAchievements { get; set; }
        public string OtherSkills { get; set; }
        public int SkillID { get; set; }
        public string Skills { get; set; }


        //newly added
        public int CurrentRRF { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime JoiningDate { get; set; }

        //public List<int> ListOfSkills
        //{



        //}







        //public int SkillID
        //{
        //    get { return SkillID; }
        //    set { SkillID = value; }
        //}

        //public string Skill
        //{
        //    get { return Skill; }
        //    set { Skill = value; }
        //}

        #endregion


        # region Experience Properties
        public int ExpID { get; set; }
        //public int OrganisationName { get; set; }
        public string OrganisationName { get; set; }
      //  public string Location { get; set; }
        public DateTime WorkedFrom { get; set; }
        public DateTime WorkedTill { get; set; }
        public string PositionHeld { get; set; }
        public string ReportingManager { get; set; }
        public int ExpType { get; set; }
        public string CTC { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }


        #endregion

        #region Education Properties
        public int EducationID { get; set; }
        public int Degree { get; set; }
        //public string PGUG { get; set; }
        public string Specialization { get; set; }
       // public string Institute { get; set; }
        public string University { get; set; }
        public int Course { get; set; }
        public int Year { get; set; }
        public int Type { get; set; }
        public string Percentage { get; set; }

        #endregion

        #region Certification Properties
        public int CertificationID { get; set; }
        public int CertificationName { get; set; }
        //public string CertificationName { get; set; }
        public string CertificationNo { get; set; }
        public string Institution { get; set; }
        public DateTime CertificationDate { get; set; }
        public string CertificationScore { get; set; }
        public string CertificationGrade { get; set; }

        #endregion



    }
    public class Skills
    {
        private int skillID;
        private string skill;
    }
}
