/* File Created: August 14, 2013 */

$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

$(document).ready(
        function () {
            empId = employeeId;
            employeeGapExpId = employeeGapExpId;
            oldStartDate = "";
            oldEndDate = "";

            $("#gapExperiencejqTable").jqGrid({
                url: window.GetgapexperiencedetailsloadgridUrl,
                postData: { employeeId: employeeId },
                datatype: "json",
                mtype: "POST",
                colNames: ["Employee Id", "EmployeeGapExp Id", "Reason", "From Date", "To Date", "Gap Duration", "Description", ""],
                colModel: [
                    { name: "EmployeeId", index: "EmployeeId", hidden: true, width: 100, align: "left" },
                    { name: "EmployeeGapExpId", index: "EmployeeGapExpId", hidden: true, width: 100, align: "left" },
                    { name: "Reason", index: "Reason", width: 150, align: "left", editable: true, editrules: { required: true } },
                    {
                        name: "FromDate", index: "FromDate", width: 70, editable: true, sorttype: 'date', editrules: { required: true }, align: "left", formatter: 'date', formatoptions: { newformat: 'm/d/Y' }, editoptions: {
                            readonly: true,
                            dataEvents: [{ type: 'change', fn: function (e) { ChangeGapFromDate(e); } }],
                            dataInit: function (element) {
                                $(element).datepicker({
                                    dateFormat: 'mm/dd/yy',
                                    changeMonth: true,
                                    changeYear: true,
                                    maxDate: 0,
                                    yearRange: "1960:+0",
                                    showOn: "both",
                                    maxdate: new Date
                                }).bind("mouseover", function () {
                                    var selectedDateID = this.id;
                                    var rowID = selectedDateID.split("_");
                                    var selectedStartDate = this.value;
                                    if (rowID[0] == "new")
                                        var endDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_ToDate").val();
                                    else
                                        var endDateValue = $("#" + rowID[0] + "_ToDate").val();
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
                        name: "ToDate", index: "ToDate", width: 70, editable: true, sorttype: 'date', editrules: { required: true }, align: "left", formatter: 'date', formatoptions: { newformat: 'm/d/Y' }, editoptions: {
                            readonly: true,
                            dataEvents: [{ type: 'change', fn: function (e) { ChangeGapToDate(e); } }],
                            dataInit: function (element) {
                                $(element).datepicker({
                                    dateFormat: 'mm/dd/yy',
                                    changeMonth: true,
                                    changeYear: true,
                                    maxDate: 0,
                                    yearRange: "1960:+30",
                                    showOn: "both",
                                    maxdate: new Date
                                }).bind("mouseover", function () {
                                    var selectedDateID = this.id;
                                    var rowID = selectedDateID.split("_");
                                    var selectedEndDate = this.value;
                                    if (rowID[0] == "new")
                                        var startDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_FromDate").val();
                                    else
                                        var startDateValue = $("#" + rowID[0] + "_FromDate").val();
                                    selectedEndDate = new Date(Date.parse(selectedEndDate, "MM/dd/yyyy"));
                                    startDateValue = new Date(Date.parse(startDateValue, "MM/dd/yyyy"));

                                    if (selectedEndDate < startDateValue && selectedEndDate != "" && startDateValue != "") {
                                        $(this).val(oldEndDate);
                                    }
                                });
                            }
                        }
                    },
                    { name: "GapDuration", index: "GapDuration", width: 70, align: "left", editable: true, classes: "DependandGapClass" },
                    { name: "Description", index: "Description", width: 140, align: "left", editable: true },
                    {
                        name: "Delete",
                        index: "Delete",
                        width: 15,
                        align: "center",
                        formatter: function () {
                            if ($('#UserRole').val() != window.RMG && $('#UserRole').val() != window.HRExecutive && (window.Empstatusmasterid != 2)) {
                                return '<img src="../../Images/New Design/delete-icon.png" width="21px" height="25px">';
                            } else {
                                return '';
                            }
                        }
                    }
                ],
                width: 600,
                toppager: false,
                jsonReader: { repeatitems: false },
                pager: $("#gapExperiencejqTablePager"),
                rowNum: 5,
                rowList: [5, 10, 20],
                viewrecords: true,
                height: 'auto',
                autowidth: false,
                //caption: "Gap Experience Details",
                editurl: window.EditGapExpDetailsURl,
                gridComplete: function () {
                    var grid = $(this).getRowData();
                    var count = jQuery("#gapExperiencejqTable").jqGrid('getGridParam', 'records');
                    if (window.empStatusId == 2) {
                        $("#gapExperiencejqTable").jqGrid('hideCol', 'Delete');
                        $("gapExperiencejqTablePager_left").css("visibility", "hidden");
                        $("#gbox_gapExperiencejqTable").find('input,select').attr("disabled", true);
                    }
                },
                onCellSelect: function (rowid, iCol) {
                    if ($('#UserRole').val() != window.HRAdmin) {
                        return false;
                    }
                    var rowData = $(this).getRowData(rowid);
                    var empGapExpId = rowData['EmployeeGapExpId'];
                    employeeGapExpId = rowData['EmployeeGapExpId'];
                    oldStartDate = rowData['FromDate'];
                    oldEndDate = rowData['ToDate'];
                    if (iCol == 7 && (window.Empstatusmasterid != 2) && rowid != "new_row") {
                        DeleteEmployeeGapExperienceDetailDialog(empGapExpId);
                    } else {
                        if (window.Empstatusmasterid != 2) {
                            //EditEmployeeGapExperienceDetails(rowData);
                        }
                    }
                }
            }).navGrid(
                "#gapExperiencejqTablePager",
                { search: false, refresh: false, add: false, edit: false, del: false },
                {},
                {},
                {}
            );
            $("#gapExperiencejqTable").jqGrid('inlineNav', "#gapExperiencejqTablePager",
    {
        edit: true,
        editicon: "ui-icon-pencil",
        add: true,
        addicon: "ui-icon-plus",
        save: true,
        saveicon: "ui-icon-disk",
        cancle: true,
        cancelicon: "ui-icon-cancel",
        addtext: "Add",
        savetext: "Save",
        edittext: "Edit",
        canceltext: "Cancel",
        addParams: {
            // position: "last",
            addRowParams: {
                // the parameters of editRow used to edit new row
                keys: true,
                oneditfunc: function (rowid) {
                    $(".DependandGapClass").find("input").attr("readonly", "readonly");
                    $('#undefined').hide();
                }
            }
        },
        editParams: {
            keys: true,
            oneditfunc: function (data, value) {
                $(".DependandGapClass").find("input").attr("readonly", "readonly");
            },
            sucessfunc: function (data) {
            },
            url: null,
            extraparam: {
                EmployeeId: function () {
                    return empId
                },
                EmployeeGapExpId: function () {
                    return employeeGapExpId
                }
            },
            aftersavefunc: function (data, response) {
                var result = $.parseJSON(response.responseText);
                if (result == true) {
                    $("#gapExperiencejqTable").trigger("reloadGrid");
                    RefreshTotalExperienceView();
                    $("#GapExperienceSuccessDialog").dialog({
                        resizable: false,
                        height: 140,
                        modal: true,
                        title: 'Experience Details',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
                else {
                    $("#GapExperienceErrorDialog").dialog({
                        resizable: false,
                        height: 140,
                        modal: true,
                        title: 'Experience Details',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            },
            errorfunc: null,
            afterrestorefunc: null,
            restoreAfterError: true,
            mtype: "POST"
        }
    })

            function ToDateChange(e) {
                var selectedDateID = e.target.id;
                var rowID = selectedDateID.split("_");
                var selectedStartDate = e.target.value;
                if (rowID[0] == "new")
                    var endDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_ToDate").val();
                else
                    var endDateValue = $("#" + rowID[0] + "_ToDate").val();

                if (selectedStartDate > endDateValue && selectedStartDate != "" && endDateValue != "") {
                    //var retToDate = todate.getFullYear() * 12 + todate.getMonth();
                    var retToDate = endDateValue.getFullYear() * 12 + endDateValue.getMonth();
                    //var retFromDate = fromDate.getFullYear() * 12 + fromDate.getMonth();
                    var retFromDate = selectedStartDate.getFullYear() * 12 + selectedStartDate.getMonth();

                    //var ret_todays = todate.getDate();
                    var ret_todays = endDateValue.getDate();

                    //var ret_fromdays = fromDate.getDate();
                    var ret_fromdays = selectedStartDate.getDate();

                    var monthDiff = (retToDate - retFromDate);
                    var daysDiff = (ret_todays - ret_fromdays);

                    var yeartotal;
                    var monthDiff1;
                    var year1;
                    var monthfinal;
                    if (monthDiff > 12) {
                        yeartotal = monthDiff / 12;
                        monthDiff1 = monthDiff % 12;
                        year1 = yeartotal.toFixed(0);
                    }
                    else {
                        year1 = 0;
                        monthDiff1 = (retToDate - retFromDate);
                    }

                    if (daysDiff > 0) {
                        monthfinal = monthDiff1 + 1;
                    }
                    else {
                        monthfinal = monthDiff1;
                    }
                    var total = year1 + " Year " + monthfinal + " Month";
                    $('#txtAddGapExperienceDetailsDuration').val(total);
                }
            }

            if ($('#UserRole').val() != HRAdmin) {
                $("#gapExperiencejqTablePager_left").css("visibility", "hidden");
                $("#gapExperiencejqTable").jqGrid('hideCol', 'Delete');
            }
        });   // end ready

var DeleteEmployeeGapExperienceDetailDialog = function (empHistoryId) {
    $('#AddGapExperienceDetailsSuccessErrorDialog').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        height: 140,
        width: 300,
        title: 'Delete Gap Experience Details',
        dialogClass: "noclose",
        buttons: {
            "Ok": function () { DeleteEmployeeGapExperienceDetailsCall(empHistoryId); },
            "Cancel": function () {
                $(this).dialog('close');
            }
        }
    });
    $("#AddGapExperienceDetailsSuccessErrorDialog").html("Are you sure you want to delete the selected record?").dialog('open');
};

function DeleteEmployeeGapExperienceDetailsCall(empHistoryId) {
    $.ajax({
        url: window.DeletegapexperiencedetailsUrl,
        data: { empGapExpId: empHistoryId },
        success: function (data) {
            if (data == true) {
                jQuery("#gapExperiencejqTable").trigger("reloadGrid");
                $("#AddGapExperienceDetailsSuccessErrorDialog").dialog('close');
                TaskGapCompletionShowDialog("Record has been deleted.");
                RefreshTotalExperienceView();
            } else {
                TaskCompletionShowDialog("Unexpected Error Occurred!");
            }
            jQuery("#gapExperiencejqTable").trigger("reloadGrid");
            $("#AddGapExperienceDetailsSuccessErrorDialog").dialog('close');
            TaskGapCompletionShowDialog("Record has been deleted.");
        },
        Error: function () { TaskCompletionShowDialog("Unexpected Error Occurred!"); }
    });
}

function TaskGapCompletionShowDialog(dialogMessage) {
    $("#DeleteGapExperienceDetailsSuccessDialog").dialog({
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
    $("#DeleteGapExperienceDetailsSuccessDialog").html(dialogMessage).dialog('open');
}

function RefreshTotalExperienceView() {
    $("#TotalExperienceDetailsMain").load(window.GetTotalExperienceDetailsurl);
}

function ChangeGapFromDate(e) {
    var selectedDateID = e.target.id;
    var rowID = selectedDateID.split("_");
    var selectedStartDate = e.target.value;
    if (rowID[0] == "new")
        var endDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_ToDate").val();
    else
        var endDateValue = $("#" + rowID[0] + "_ToDate").val();
    selectedStartDate = new Date(Date.parse(selectedStartDate, "MM/dd/yyyy"));
    endDateValue = new Date(Date.parse(endDateValue, "MM/dd/yyyy"));
    if (selectedStartDate > endDateValue && selectedStartDate != "" && endDateValue != "") {
        $("#FromDateGreaterDialog").dialog({
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
                            $("#" + rowID[0] + "_" + rowID[1] + "_FromDate").val(oldStartDate);
                        else
                            $("#" + rowID[0] + "_FromDate").val(oldStartDate);
                    }
                }
        });
    }
}

function humanise(diff) {
    var str = '';
    var values = {
        ' year': 365,
        ' month': 30,
        ' day': 1
    };

    for (var x in values) {
        var amount = Math.floor(diff / values[x]);

        if (amount >= 1) {
            str += amount + x + (amount > 1 ? 's' : '') + ' ';
            diff -= amount * values[x];
        }
    }

    return str;
}

function ChangeGapToDate(e) {
    var selectedDateID = e.target.id;
    var rowID = selectedDateID.split("_");
    var selectedEndDate = e.target.value;
    if (rowID[0] == "new")
        var startDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_FromDate").val();
    else
        var startDateValue = $("#" + rowID[0] + "_FromDate").val();
    startDateValue = new Date(Date.parse(startDateValue, "MM/dd/yyyy"));
    selectedEndDate = new Date(Date.parse(selectedEndDate, "MM/dd/yyyy"));

    var start_date = new Date(startDateValue),
    end_date = new Date(selectedEndDate),
    one_day_in_milliseconds = 1000 * 60 * 60 * 24,
    date_diff = Math.floor((end_date.getTime() - start_date.getTime()) / one_day_in_milliseconds);
    var DateToDisplay = humanise(date_diff);

    $("#" + rowID[0] + "_GapDuration").val(DateToDisplay);
    $("#new_row_GapDuration").val(DateToDisplay);

    if (selectedEndDate < startDateValue && selectedEndDate != "" && startDateValue != "") {
        $("#TillDateLesserDialog").dialog({
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
                            $("#" + rowID[0] + "_" + rowID[1] + "_ToDate").val(oldEndDate);
                        else
                            $("#" + rowID[0] + "_ToDate").val(oldEndDate);
                    }
                }
        });
    }
}