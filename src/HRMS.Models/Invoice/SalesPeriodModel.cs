using System;
using System.Collections.Generic;

namespace HRMS.Models
{
    public class SalesPeriodModel
    {
        public SearchedUserDetails SearchedUserDetails { get; set; }
        public string UserName { get; set; }

        public int SalesPeriodMonthID { get; set; }
        public List<SalesPeriodMonthList> SalesPeriodMonthLists { get; set; }

        public int SalesPeriodYearID { get; set; }
        public List<SalesPeriodYearList> SalesPeriodYearLists { get; set; }

        public int SalesPeriodIsOpenID { get; set; }
        public List<SalesPeriodIsOpenList> SalesPeriodIsOpenLists { get; set; }
    }

    public class AddSalesPeriods
    {
        public int SalesPeriodID { get; set; }
        public bool isOpenID { get; set; }
        public DateTime? SalesPeriodStartDate { get; set; }
        public DateTime? SalesPeriodEndDate { get; set; }
        public string SalesPeriodMonth { get; set; }
        public int? SalesPeriodYear { get; set; }
        public string SalesPeriodIsOpen { get; set; }
        public int? HiddenSalesPeriodMonthID { get; set; }
        public int? HiddenSalesPeriodYearID { get; set; }
        public int? HiddenSalesPeriodIsOpenID { get; set; }
    }

    public class SalesPeriodMonthList
    {
        public int SalesPeriodMonthID { get; set; }
        public int? SalesPeriodMonth { get; set; }
    }

    public class SalesPeriodYearList
    {
        public int? SalesPeriodYearID { get; set; }
        public string SalesPeriodYear { get; set; }
    }

    public class SalesPeriodIsOpenList
    {
        public int SalesPeriodIsOpenID { get; set; }
        public string SalesPeriodIsOpen { get; set; }
    }
}