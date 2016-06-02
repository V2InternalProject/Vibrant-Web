using System.Collections.Generic;
using System.Data.SqlClient;

namespace HRMS.Models
{
    public class ReportModel
    {
        public int ReportID { get; set; }
        public string SelectedGuid { get; set; }
        public List<Report> reportList { get; set; }
    }

    public class Report
    {
        public int EmployeeId { get; set; }
        public int ReportID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string ReportFileName { get; set; }
    }

    public class ReportParamterMaster
    {
        public int ReportParamID { get; set; }
        public int? ReportID { get; set; }
        public string ReportParamName { get; set; }
        public string ReportParamType { get; set; }
        public string ReportParamDefaultValue { get; set; }
        public string ReportParamDescription { get; set; }
        public string ReportParamSP { get; set; }
        public List<DropDownValues> DropDownList { get; set; }
        public string CascadeDD { get; set; }
        public string All { get; set; }
        public string Team { get; set; }
    }

    public class DropDownValues
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ControlsList
    {
        public string id { get; set; }
        public string value { get; set; }
        public string TypeId { get; set; }
    }

    public class ReportSchema
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string ProcName { get; set; }
        public List<SqlParameter> ProcInputItemList { get; set; }
    }

    public class ReportMaster
    {
        public int ReportID { get; set; }
        public string ReportDatasetName { get; set; }
    }
}