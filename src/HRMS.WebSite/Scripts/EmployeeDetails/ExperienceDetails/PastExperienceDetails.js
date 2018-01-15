///* File Created: August 14, 2013 */

/* File Created: August 14, 2013 */
$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

$(document).ready(

        function () {
            var selectedStartDate = null;
            var endDateValue = null;
            //$('#EmployeeId').val(pastExperienceEmployeeId);
            empId = employeeId;
            employeeHistoryId = employeeHistoryId;
            SelectedEmpTypeRowId = "";
            var SelectedRowId = null;
            typeList = EmployeeWorkingTypeList;
            var TypeList = [];
            TypeList.push("Select");
            $.each(typeList, function (index, value) {
                TypeList.push(value.WorkingTypeName);
            });
            $("#pastExperiencejqTable").jqGrid({
                url: window.Getpastexperiencedetailsloadgrid,
                postData: { employeeId: employeeId },
                datatype: "json",
                mtype: "POST",
                colNames: ["Organization Name", "Location", "Worked From", "Worked Till", "Employee History Id", "Type", "Working Type Id", "Last Designation", "Reporting Manager", "Last Salary Drawn (LPA)", ""],
                colModel: [
                    { name: "OrganizationName", index: "OrganizationName", width: 100, align: "left", editable: true, editrules: { required: true } },
                    { name: "Location", index: "Location", width: 100, align: "left", editable: true, editrules: { required: true } },
                   {
                       name: "WorkedFrom", index: "WorkedFrom", width: 70, editable: true, sorttype: 'date', editrules: { required: true }, align: "left", formatter: 'date', formatoptions: { newformat: 'm/d/Y' }, editoptions: {
                           readonly: true,
                           dataEvents: [{ type: 'change', fn: function (e) { ChangePastFromDate(e); } }],
                           dataInit: function (element) {
                               $(element).datepicker({
                                   dateFormat: 'mm/dd/yy',
                                   changeMonth: true,
                                   changeYear: true,
                                   maxDate: 0,
                                   yearRange: "1960:+30",
                                   showOn: "both",
                                   beforeShow: BeforeShowForDatePicker,
                                   onClose: onCloseForDatePicker,
                                   buttonImage: "../../Images/New Design/calender-icon.png", buttonImageOnly: true
                               }).bind("mouseover", function () {
                                   var selectedDateID = this.id;
                                   var rowID = selectedDateID.split("_");
                                   selectedStartDate = this.value;
                                   if (rowID[0] == "new")
                                       endDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_WorkedTill").val();
                                   else
                                       endDateValue = $("#" + rowID[0] + "_WorkedTill").val();
                                   selectedStartDate = new Date(Date.parse(selectedStartDate, "MM/dd/yyyy"));
                                   endDateValue = new Date(Date.parse(endDateValue, "MM/dd/yyyy"));
                                   if (selectedStartDate > endDateValue && selectedStartDate != "" && endDateValue != "") {
                                       $(this).val(oldStartDate);
                                   }
                               });
                           }
                       }
                   },
                    {
                        name: "WorkedTill", index: "WorkedTill", width: 70, editable: true, sorttype: 'date', editrules: { required: true }, align: "left", formatter: 'date', formatoptions: { newformat: 'm/d/Y' }, editoptions: {
                            readonly: true,
                            dataEvents: [{
                                type: 'change', fn: function (e) {
                                    // var minValue = $(this).val();
                                    // minValue = $.datepicker.parseDate("mm/dd/yy", minValue);
                                    // minValue.setDate(minValue.getDate() + 1);
                                    ChangePastToDate(e);
                                }
                            }],
                            dataInit: function (element) {
                                $(element).datepicker({
                                    dateFormat: 'mm/dd/yy',
                                    changeMonth: true,
                                    changeYear: true,
                                    maxDate: new Date,
                                    yearRange: "1960:+0",
                                    showOn: "both",
                                    beforeShow: BeforeShowForDatePicker,
                                    onClose: onCloseForDatePicker,
                                    buttonImage: "../../Images/New Design/calender-icon.png", buttonImageOnly: true,
                                    // mindate: minValue,
                                    //beforeShowDay: disableRangeOfDays
                                }).bind("mouseover", function () {
                                    var selectedDateID = this.id;
                                    var rowID = selectedDateID.split("_");
                                    var selectedEndDate = this.value;
                                    if (rowID[0] == "new")
                                        var startDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_WorkedFrom").val();
                                    else
                                        var startDateValue = $("#" + rowID[0] + "_WorkedFrom").val();

                                    selectedStartDate = new Date(selectedStartDate, "MM/dd/yyyy");
                                    endDateValue = new Date(endDateValue, "MM/dd/yyyy");

                                    if (selectedEndDate < startDateValue && selectedEndDate != "" && startDateValue != "") {
                                        $(this).val(oldEndDate);
                                    }
                                });
                            }
                        }
                    },
                    { name: "EmployeeHistoryId", index: "EmployeeHistoryId", hidden: true, width: 100, align: "left" },
                    { name: "EmployeeWorkingType", index: "EmployeeWorkingType", align: "left", width: 60, editable: true, editrules: { required: true, custom: true, custom_func: IsTypeSelected }, edittype: "select", editoptions: { value: TypeList, dataEvents: [{ type: 'change', fn: function (e) { getTypesList(e); } }] } },
                   { name: "EmployeeTypeId", index: "EmployeeTypeId", hidden: true, width: 100, align: "left" },
                    { name: "LastDesignation", index: "LastDesignation", width: 120, align: "left", editable: true, required: true },
                    { name: "ReportingManager", index: "ReportingManager", width: 120, align: "left", editable: true, editrules: { custom: true, custom_func: isValidName } },
                    { name: "LastSalaryDrawn", index: "LastSalaryDrawn", width: 100, align: "left", editable: true, editrules: { custom: true, custom_func: isValidNumber } },
                    {
                        name: "Delete",
                        index: "Delete",
                        width: 30,
                        align: "left",
                        formatter: function () {
                            if ($('#UserRole').val() != window.RMG && $('#UserRole').val() != window.HRExecutive && (window.Empstatusmasterid != 2)) {
                                return '<img src="../../Images/New Design/delete-icon.png" width="21px" height="25px">';
                            } else {
                                return '';
                            }
                        }
                    }
                ],
                width: 800,
                toppager: false,
                jsonReader: { repeatitems: false },
                pager: $("#pastExperiencejqTablePager"),
                rowNum: 5,
                rowList: [5, 10, 20],
                viewrecords: true,
                autowidth: false,
                height: 'auto',
                //caption: "Past Experience Details",
                editurl: window.EditPastExpDetailsURl,
                gridComplete: function () {
                    var grid = $(this).getRowData();
                    var count = jQuery("#pastExperiencejqTable").jqGrid('getGridParam', 'records');
                    //if (count == 0) {
                    //    $('#gview_pastExperiencejqTable').hide();
                    //    $("#pastExpDetailsContainer").append("<p>No records found</p>")
                    //}
                    if (window.empStatusId == 2) {
                        $("#pastExperiencejqTable").jqGrid('hideCol', 'Delete');
                        $("pastExperiencejqTablePager_left").css("visibility", "hidden");
                        $("#gbox_pastExperiencejqTable").find('input,select').attr("disabled", true);
                    }
                },
                onCellSelect: function (rowid, iCol) {
                    if ($('#UserRole').val() != window.HRAdmin) {
                        return false;
                    }
                    var rowData = $(this).getRowData(rowid);
                    var empHistoryId = rowData['EmployeeHistoryId'];
                    employeeHistoryId = rowData['EmployeeHistoryId'];
                    if (iCol == 10 && (window.Empstatusmasterid != 2) && rowid != "new_row") {
                        DeleteEmployeePastExperienceDetailDialog(empHistoryId);
                    } else {
                        if (window.Empstatusmasterid != 2) {
                            //EditEmployeePastExperienceDetails(rowData);
                        }
                    }
                }
            }).navGrid(
                "#pastExperiencejqTablePager",
                { search: false, refresh: false, add: false, edit: false, del: false },
                {},
                {},
                {}
            );
      //      $("#pastExperiencejqTable").jqGrid('inlineNav', "#pastExperiencejqTablePager",
      //{
      //    edit: true,
      //    editicon: "ui-icon-pencil",
      //    add: true,
      //    addicon: "ui-icon-plus",
      //    save: true,
      //    saveicon: "ui-icon-disk",
      //    cancle: true,
      //    cancelicon: "ui-icon-cancel",
      //    addtext: "Add",
      //    savetext: "Save",
      //    edittext: "Edit",
      //    canceltext: "Cancel",
      //    addParams: {
      //        // position: "last",
      //        addRowParams: {
      //            // the parameters of editRow used to edit new row
      //            keys: true,
      //            oneditfunc: function (rowid) {
      //                $('#undefined').hide();
      //            }
      //        }
      //    },
      //    editParams: {
      //        keys: true,
      //        oneditfunc: function (data, value) {
      //        },
      //        sucessfunc: function (data) {
      //        },
      //        url: null,
      //        extraparam: {
      //            EmployeeId: function () {
      //                return empId
      //            },
      //            EmpHistroyId: function () {
      //                return employeeHistoryId
      //            },
      //            EmpTypeId: function () {
      //                return $('#EmployeeTypeId').val();
      //            }
      //        },
      //        aftersavefunc: function (data, response) {
      //            var result = $.parseJSON(response.responseText);
      //            if (result == true) {
      //                $("#pastExperiencejqTable").trigger("reloadGrid");
      //                RefreshTotalExperienceView();
      //                $("#pastExperienceSuccessDialog").dialog({
      //                    resizable: false,
      //                    height: 140,
      //                    modal: true,
      //                    dialogClass: "noclose",
      //                    title: 'Experience Details',
      //                    buttons: {
      //                        Ok: function () {
      //                            $(this).dialog("close");
      //                        }
      //                    }
      //                });
      //            }
      //            else {
      //                $("#pastExperienceErrorDialog").dialog({
      //                    resizable: false,
      //                    height: 140,
      //                    modal: true,
      //                    dialogClass: "noclose",
      //                    title: 'Experience Details',
      //                    buttons: {
      //                        Ok: function () {
      //                            $(this).dialog("close");
      //                        }
      //                    }
      //                });
      //            }
      //        },
      //        errorfunc: null,
      //        afterrestorefunc: null,
      //        restoreAfterError: true,
      //        mtype: "POST"
      //    }
      //});

            if ($('#UserRole').val() != HRAdmin) {
                $("#pastExperiencejqTablePager_left").css("visibility", "hidden");
                $("#pastExperiencejqTable").hideCol("LastSalaryDrawn");
                $("#pastExperiencejqTable").jqGrid('hideCol', 'Delete');
            }
        });  //ready end

function isValidNumber(value, Colname) {
    var pattern = new RegExp(/^\+?[0-9]*\.?[0-9]+$/);
    var valid = pattern.test(value);
    if (!valid) {
        $("#NumberDialog").dialog({
            modal: true,
            resizable: false,
            height: 'auto',
            width: 300,
            title: "Info",
            buttons:
            {
                "Ok": function () {
                    $(this).dialog("close");
                }
            }
        });
        $.preventDefault();
    }
    else {
        return [true, ""];
    }
}

function y2k(number) { return (number < 1000) ? number + 1900 : number; }

function calculateDate(startDate, endDate) {
}

function isValidName(value, Colname) {
    var pattern = new RegExp(/^[a-zA-Z ]*$/);
    var valid = pattern.test(value);
    if (!valid) {
        $("#CharacterDialog").dialog({
            modal: true,
            resizable: false,
            height: 'auto',
            width: 300,
            title: "Info",
            buttons:
            {
                "Ok": function () {
                    $(this).dialog("close");
                }
            }
        });
        $.preventDefault();
    }
    else {
        return [true, ""];
    }
}

function ChangeWorkedFromDate(e) {
    var selectedDateID = e.target.id;
    var rowID = selectedDateID.split("_");
    var selectedStartDate = e.target.value;
    if (rowID[0] == "new")
        var endDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_WorkedTill").val();
    else
        var endDateValue = $("#" + rowID[0] + "_WorkedTill").val();
    if (selectedStartDate > endDateValue && selectedStartDate != "" && endDateValue != "") {
        $("#WorkedFromDateGreaterDialog").dialog({
            modal: true,
            resizable: false,
            height: 140,
            width: 300,
            dialogClass: "noclose",
            buttons:
            {
                "Ok": function () {
                    $(this).dialog("close");
                    if (rowID[0] == "new")
                        var endDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_WorkedFrom").val(oldStartDate);
                    else
                        var endDateValue = $("#" + rowID[0] + "_WorkedFrom").val(oldStartDate);
                    //$("#"+rowID[0]+"_StartDate").val(oldStartDate);
                }
            }
        });
    }
}

function ChangeWorkedTillDate(e) {
    var selectedDateID = e.target.id;
    var rowID = selectedDateID.split("_");
    var selectedEndDate = e.target.value;
    if (rowID[0] == "new")
        var startDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_WorkedFrom").val();
    else
        var startDateValue = $("#" + rowID[0] + "_WorkedFrom").val();
    //var startDateValue = $("#"+rowID[0]+"_StartDate").val();
    if (selectedEndDate < startDateValue && selectedEndDate != "" && startDateValue != "") {
        $("#WorkedTillDateLesserDialog").dialog({
            modal: true,
            resizable: false,
            height: 140,
            width: 300,
            dialogClass: "noclose",
            buttons:
            {
                "Ok": function () {
                    $(this).dialog("close");
                    if (rowID[0] == "new")
                        var endDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_WorkedTill").val(oldEndDate);
                    else
                        var endDateValue = $("#" + rowID[0] + "_WorkedTill").val(oldEndDate);
                    //$("#"+rowID[0]+"_EndDate").val(oldEndDate);
                }
            }
        });
    }
}

function IsTypeSelected(value, colname) {
    if (value == "0") {
        $("#RequiredFeildForType").dialog({
            modal: true,
            resizable: false,
            height: 140,
            width: 300,
            dialogClass: "noclose",
            buttons:
            {
                "Ok": function () {
                    $(this).dialog("close");
                }
            }
        });
        $.preventDefault();
    }
    else {
        return [true, ""];
    }
}

function getTypesList(e) {
    var type = e.target[e.target.selectedIndex].text;
    var ID;
    $.each(typeList, function (index, value) {
        var empName = value.WorkingTypeName.replace('  ', ' ');

        if (empName == type) {
            ID = value.EmployeeTypeId;
        }
    });
    $('#EmployeeTypeId').val(ID);

    $('#' + SelectedCertificationRowId + '_CertificationName').attr('title', type);
}

var DeleteEmployeePastExperienceDetailDialog = function (empHistoryId) {
    $('#AddPastExperienceDetailsSuccessErrorDialog').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        height: 140,
        width: 300,
        dialogClass: "noclose",
        title: 'Delete Experience Details',
        buttons: {
            "Ok": function () { DeleteEmployeePastExperienceDetailsCall(empHistoryId); }
           ,
            "Cancel": function () {
                $(this).dialog('close');
            }
        }
    });
    $("#AddPastExperienceDetailsSuccessErrorDialog").html("Are you sure you want to delete the selected record?").dialog('open');
};

function DeleteEmployeePastExperienceDetailsCall(empHistoryId) {
    $.ajax({
        url: window.detailPastExperience,
        data: { empHistoryId: empHistoryId },
        success: function (data) {
            if (data == true) {
                jQuery("#pastExperiencejqTable").trigger("reloadGrid");
                $("#AddPastExperienceDetailsSuccessErrorDialog").dialog('close');
                TaskCompletionShowDialog("Record has been deleted.");
                RefreshTotalExperienceView();
            } else {
                TaskCompletionShowDialog("Unexpected Error Occurred!");
            }

            jQuery("#pastExperiencejqTable").trigger("reloadGrid");
            $("#AddPastExperienceDetailsSuccessErrorDialog").dialog('close');
            TaskCompletionShowDialog("Record has been deleted.");
        },
        Error: function () { TaskCompletionShowDialog("Unexpected Error Occurred!"); }
    });
}

function TaskCompletionShowDialog(dialogMessage) {
    $("#AddPastExperienceDetailsSuccessErrorDialog").dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        height: 140,
        width: 300,
        title: 'Deleted',
        dialogClass: "noclose",
        //   position: [400, 200],
        buttons: {
            "Ok": function () {
                $(this).dialog('close');
            }
        }
    });
    $("#AddPastExperienceDetailsSuccessErrorDialog").html(dialogMessage).dialog('open');
}

function RefreshTotalExperienceView() {
    $("#TotalExperienceDetailsMain").load(window.TotalExperienceDetailsMainUrl);
}

var AddPastExperienceDetails = function () {
    $('#frmAddPastExperienceDetails #txtAddPastExperienceDetailsOrganizationName').val('');
    $('#frmAddPastExperienceDetails #txtAddPastExperienceDetailsLocation').val('');
    $('#frmAddPastExperienceDetails #txtAddNewWorkedFrom').val('');
    $('#frmAddPastExperienceDetails #txtAddNewWorkedTill').val('');
    $('#frmAddPastExperienceDetails #ddlPastExperienceDetailsEmployeeType').val('');
    $('#frmAddPastExperienceDetails #hdnAddExperienceDetailsEmployeeHistoryId').val(0);
    $('#frmAddPastExperienceDetails #hdnAddExperienceDetailsEmployeeId').val(pastExperienceEmployeeId);
    $('#frmAddPastExperienceDetails #txtAddPastExperienceDetailsLastDesignation').val('');
    $('#frmAddPastExperienceDetails #txtAddPastExperienceDetailsReportingManager').val('');
    $('#frmAddPastExperienceDetails #txtAddPastExperienceDetailsLastSalaryDrawn').val('');

    //for hidden fields
    $('#frmAddPastExperienceDetails #organizationName').val('');
    $('#frmAddPastExperienceDetails #location').val('');
    $('#frmAddPastExperienceDetails #workedFrom').val('');
    $('#frmAddPastExperienceDetails #workedTill').val('');
    $('#frmAddPastExperienceDetails #employeeTypeId').val('');
    //$('#frmAddPastExperienceDetails #employeeTypeId').val(1);
    $('#frmAddPastExperienceDetails #LastDesignation').val('');
    $('#frmAddPastExperienceDetails #ReportingManager').val('');
    $('#frmAddPastExperienceDetails #LastSalaryDrawn').val('');
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $('#pastNewExperienceDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: 'Past Experience Details',
        close: function () {
            $(".field-validation-error").empty();
            $('input').removeClass("input-validation-error");
        }
    });
    $('#pastNewExperienceDialog').dialog('open');
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
};

var EditEmployeePastExperienceDetails = function (object) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $('#frmAddPastExperienceDetails #txtAddPastExperienceDetailsOrganizationName').val(object['OrganizationName']);
    $('#frmAddPastExperienceDetails #txtAddPastExperienceDetailsLocation').val(object['Location']);
    $('#frmAddPastExperienceDetails #txtAddNewWorkedFrom').val(object['WorkedFrom']);
    $('#frmAddPastExperienceDetails #txtAddNewWorkedTill').val(object['WorkedTill']);
    $('#frmAddPastExperienceDetails #hdnAddExperienceDetailsEmployeeHistoryId').val(object['EmployeeHistoryId']);
    $('#frmAddPastExperienceDetails #ddlPastExperienceDetailsEmployeeType').val(object['EmployeeTypeId']);
    $('#frmAddPastExperienceDetails #hdnAddExperienceDetailsEmployeeId').val(pastExperienceEmployeeId);
    $('#frmAddPastExperienceDetails #txtAddPastExperienceDetailsLastDesignation').val(object['LastDesignation']);
    $('#frmAddPastExperienceDetails #txtAddPastExperienceDetailsReportingManager').val(object['ReportingManager']);
    $('#frmAddPastExperienceDetails #txtAddPastExperienceDetailsLastSalaryDrawn').val(object['LastSalaryDrawn']);

    //for hidden fields
    $('#frmAddPastExperienceDetails #organizationName').val(object['OrganizationName']);
    $('#frmAddPastExperienceDetails #location').val(object['Location']);
    $('#frmAddPastExperienceDetails #workedFrom').val(object['WorkedFrom']);
    $('#frmAddPastExperienceDetails #workedTill').val(object['WorkedTill']);
    $('#frmAddPastExperienceDetails #employeeTypeId').val(object['EmployeeTypeId']);
    $('#frmAddPastExperienceDetails #LastDesignation').val(object['LastDesignation']);
    $('#frmAddPastExperienceDetails #ReportingManager').val(object['ReportingManager']);
    $('#frmAddPastExperienceDetails #LastSalaryDrawn').val(object['LastSalaryDrawn']);

    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $('#pastNewExperienceDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,

        title: 'Past Experience Details',
        close: function () {
            $(".field-validation-error").empty();
            $('input').removeClass("input-validation-error");
        }
    });

    $('#pastNewExperienceDialog').dialog('open');
};

if ($('#UserRole').val() != HRAdmin) {
    $("#AddPastExperienceDetails").attr("disabled", true);
    $("#AddPastExperienceDetails").hide();
    $("#pastExperiencejqTable").hideCol("LastSalaryDrawn");
}

if ($('#UserRole').val() != HRAdmin) {
    //$("#AddPastExperienceDetails").attr("disabled", true);
    $("#AddPastExperienceDetails").hide();
    $("#pastExperiencejqTable").hideCol("LastSalaryDrawn");
}
$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

function ChangePastFromDate(e) {
    var selectedDateID = e.target.id;
    var rowID = selectedDateID.split("_");
    var selectedStartDate = e.target.value;
    if (rowID[0] == "new") {
        var endDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_WorkedTill").val();
        endDateValue = new Date(Date.parse(endDateValue, "MM/dd/yyyy"));
        startDateValue = new Date(Date.parse(selectedStartDate, "MM/dd/yyyy"));
    }
    else {
        var endDateValue = $("#" + rowID[0] + "_WorkedTill").val();
        startDateValue = new Date(Date.parse(selectedStartDate, "MM/dd/yyyy"));
        endDateValue = new Date(Date.parse(endDateValue, "MM/dd/yyyy"));
    }

    if (startDateValue > endDateValue && startDateValue != "" && endDateValue != "") {
        $("#WorkedFromDateGreaterDialog").dialog({
            modal: true,
            resizable: false,
            height: 'auto',
            width: 300,
            dialogClass: "noclose",
            buttons:
                {
                    "Ok": function () {
                        $(this).dialog("close");
                        if (rowID[0] == "new")
                            $("#" + rowID[0] + "_" + rowID[1] + "_WorkedFrom").val(oldStartDate);
                        else
                            $("#" + rowID[0] + "_FromDate").val(oldStartDate);
                    }
                }
        });
    }
}
function ChangePastToDate(e) {
    var selectedDateID = e.target.id;
    var rowID = selectedDateID.split("_");
    var selectedEndDate = e.target.value;
    if (rowID[0] == "new") {
        var startDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_WorkedFrom").val();
        startDateValue = new Date(Date.parse(startDateValue, "MM/dd/yyyy"));
        selectedEndDate = new Date(Date.parse(selectedEndDate, "MM/dd/yyyy"));
        //var spd = startDateValue.split("/");
        //var spE = selectedEndDate.split("/");
        //var s = new Date(spd[2], spd[0] - 1, spd[1]);
        //var e = new Date(spE[2], spE[0] - 1, spE[1]);
    }
    else {
        var startDateValue = $("#" + rowID[0] + "_WorkedFrom").val();
        startDateValue = new Date(Date.parse(startDateValue, "MM/dd/yyyy"));
        selectedEndDate = new Date(Date.parse(selectedEndDate, "MM/dd/yyyy"));
        //var spd = startDateValue.split("/");
        //var spE = selectedEndDate.split("/");
        //var s = new Date(spd[2], spd[0] - 1, spd[1]);
        //var e = new Date(spE[2], spE[0] - 1, spE[1]);
    }
    if (selectedEndDate < startDateValue && selectedEndDate != "" && startDateValue != "") {
        $("#WorkedTillDateLesserDialog").dialog({
            modal: true,
            resizable: false,
            height: 'auto',
            width: 300,
            dialogClass: "noclose",
            buttons:
                {
                    "Ok": function () {
                        $(this).dialog("close");
                        if (rowID[0] == "new")
                            $("#" + rowID[0] + "_" + rowID[1] + "_WorkedTill").val(oldEndDate);
                        else
                            $("#" + rowID[0] + "_WorkedTill").val(oldEndDate);
                    }
                }
        });
    }
}