using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    //Id {Auto Generated }
    //Type TASK_STATUS/TASK_TYPE {status,}
    //Value { Open ,Closed ,etc}
    //Level S/P {Global Or Project Realyed ID Will Be Reference Later}
    //referenceKey { auto poupulate projectId and will Save it }

    public class PmsConfiguration
    {
        //
        // Data For Form
        //
        public string DropDown { get; set; }       //

        public int ID { get; set; }
        public string MainType { get; set; }
        public string TypeValue { get; set; }
        public int? Rfid { get; set; }
        public string LevelType { get; set; }
        public string ProjectName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public SearchedUserDetails SearchedUserDetails { get; set; }
        public List<ProjectAppList> TimesheetProjectList { get; set; }
        public List<TimesheetSettingList> TimesheetSettingList { get; set; }
        public List<TimesheetApproverList> TimesheetApproverList { get; set; }
        public int? ProjectID { get; set; }
        public string UserName { get; set; }
        public string SettingID { get; set; }
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Value field is required")]
        public string DropDownValue { get; set; }

        [Required(ErrorMessage = "Value field is required")]
        public int? SelectedDropDownValue { get; set; }

        public string SelectedDDValue { get; set; }

        [Required(ErrorMessage = "Value field is required")]
        [RegularExpression(@"^[a-zA-Z0-9_.]*@[a-zA-Z0-9_.]*\.([a-zA-Z]{2,4})$", ErrorMessage = "Please enter valid email id.")]
        public string EmailIDValue { get; set; }

        [Required(ErrorMessage = "Value field is required")]
        public string UserValue { get; set; }

        public bool CheckBoxValue { get; set; }

        public int LookUpTypeId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string LevelName { get; set; }
        public int Ref_Id { get; set; }
        public string dataType { get; set; }
        public string dataTypeValue { get; set; }
        public string DataTypeLabel { get; set; }
    }

    public class ProjecDetails
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
    }

    public class TimesheetSettingList
    {
        public string Settingid { get; set; }
        public string SettingName { get; set; }
    }

    public class TimesheetApproverList
    {
        public int TimesheetApproverID { get; set; }
        public string TimesheetApproverName { get; set; }
    }
}