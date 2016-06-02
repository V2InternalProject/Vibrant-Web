namespace HRMS.Models
{
    public class CountryDetails
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CC_FIPS { get; set; }
    }

    public class CityDetails
    {
        public int Cityid { get; set; }
        public string CityName { get; set; }
        public string CC_FIPS { get; set; }
    }
}