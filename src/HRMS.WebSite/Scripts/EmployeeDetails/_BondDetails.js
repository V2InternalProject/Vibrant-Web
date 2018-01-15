/* File Created: August 14, 2013 */

$(document).ready(function () {
    selectedRowId = 0;
    selectedBondStatus = "";
    BondStatusForValidation = "";
    bondtypeList = empbondtypeList;
    var BondTypeList = [];
    BondTypeList.push("Select");
    $.each(bondtypeList, function (index, value) {
        BondTypeList.push(value.BondType);
    });

    bondstatusList = empbondstatusList;
    var BondStatusList = [];
    BondStatusList.push("Select");
    $.each(bondstatusList, function (index, value) {
        BondStatusList.push(value);
    });

    $("#jqBondDetailsTable").jqGrid({
        // Ajax related configurations
        url: window.loanBondDetailGridUrl,
        postData: { employeeId: bondEmployeeId },
        datatype: "json",
        mtype: "POST",
        // Specify the column names
        colNames: ["Bond ID", "Bond Type", "BondType ID", "Bond Status", "", "Bond Amount", "Bond Over Date", "Employee ID", ""],
        // Configure the columns
        colModel: [
                { name: "BondId", index: "BondId", hidden: true, width: 100, align: "left" },
                { name: "BondType", index: "BondType", align: "left", width: 100, editable: true, editrules: { required: true, custom: true, custom_func: IsBondTypeSelected }, edittype: "select", editoptions: { value: BondTypeList, dataEvents: [{ type: 'change', fn: function (e) { getBondType(e); } }] } },

               // { name: "BondType", index: "BondType", width: 100, align: "left", editable: true, editrules: { required: true, custom: true, custom_func: IsBondTypeSelected }, edittype: "select", editoptions: { value: BondTypeList, dataevents: [{ type: 'change', fn: function (e) { getBondTypeList(e); } }] } },
                { name: "BondTypeID", index: "BondTypeID", hidden: true, width: 100, align: "left" },
               // { name: "BondStatus", index: "BondStatus", width: 100, align: "left", editable: true, editrules: { required: true, custom: true, custom_func: IsBondStatusSelected }, edittype: "select", editoptions: { value: BondStatusList, dataevents: [{ type: 'change', fn: function (e) { getBondStatusList45(e); } }] } },
              { name: "BondStatus", index: "BondStatus", align: "left", width: 100, editable: true, editrules: { required: true, custom: true, custom_func: IsBondStatusSelected }, edittype: "select", editoptions: { value: BondStatusList, dataEvents: [{ type: 'change', fn: function (e) { getBondStatus(e); } }] } },
                 { name: "BondStatusHidden", index: "BondStatusHidden", hidden: true, width: 50, align: "left" },
               { name: "BondAmount", index: "BondAmount", width: 100, align: "left", editable: true, editrules: { custom: true, custom_func: IsBondAmountRequired, custom_func: isValidAmount } },
                {
                    name: "BondOverDate", index: "BondOverDate", width: 50, align: "left", editable: true, editrules: { custom: true, custom_func: IsBondOverDateRequired }, formatter: 'date', formatoptions: { newformat: 'm/d/Y' }, editoptions: {
                        readonly: true,
                        dataEvents: [{ type: 'change', fn: function (e) { } }],
                        dataInit: function (element) {
                            $(element).datepicker({
                                dateFormat: 'mm/dd/yy',
                                changeMonth: true,
                                changeYear: true,
                                beforeShow: BeforeShowForDatePicker,
                                onClose: onCloseForDatePicker,
                                buttonImage: "../../Images/New Design/calender-icon.png", buttonImageOnly: true,
                            }).bind("mouseover", function () {
                                //$(this).datepicker("option", "minDate", ProjectStartDate);
                                //$(this).datepicker("option", "maxDate", ProjectEndDate);
                                var selectedDateID = this.id;
                                var rowID = selectedDateID.split("_");
                                var selectedEndDate = this.value;
                                //if (rowID[0] == "new")
                                //    var startDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_StartDate").val();
                                //else
                                //    var startDateValue = $("#" + rowID[0] + "_StartDate").val();
                                //if (selectedEndDate < startDateValue && selectedEndDate != "" && startDateValue != "") {
                                //    $(this).val(oldEndDate);
                                //}
                            });
                        }
                    }
                },
                { name: "EmployeeId", index: "EmployeeId", hidden: true, width: 80, align: "left" },
                {
                    name: "Delete",
                    index: "Delete",
                    width: 22,
                    align: "center",
                    formatter: function () {
                        if ($('#UserRole').val() != window.RMGRole && $('#UserRole').val() != window.HRExecutive && (window.empStatusId != 2)) {
                            return '<img src="../../Images/New Design/delete-icon.png" width="21px" height="25px">';
                        } else {
                            return '';
                        }
                    }
                }
        ],

        // Grid total width and height

        width: 700,
        // Paging

        jsonReader: { repeatitems: false },
        toppager: false,
        rowNum: 5,
        rowList: [5, 10, 20],
        //viewrecords: true, // Specify if "total number of records" is displayed
        height: 'auto',
        autowidth: false,
        pager: $("#jqTableBondDetailsPager"),
        // Grid caption
        //caption: "Employee - Bond Details",
        editurl: window.editBondUrl,
        gridComplete: function () {
            var grid = $(this).getRowData();
            var count = jQuery("#jqBondDetailsTable").jqGrid('getGridParam', 'records');
            if (count == 0)
                $('#totalcount').text("No records found");
            else
                $('#totalcount').text("Total Records : " + count);

            if (window.empStatusId == 2) {
                $("#jqBondDetailsTable").jqGrid('hideCol', 'Delete');
                $("jqTableBondDetailsPager_left").css("visibility", "hidden");
                $("#gbox_jqBondDetailsTable").find('input,select').attr("disabled", true);
            }
        },
        onCellSelect: function (rowid, iCol) {
            if ($('#UserRole').val() != window.hrAdmin) {
                return false;
            }
            var rowData = $(this).getRowData(rowid);
            var selectedBondId = rowData['BondId'];
            selectedRowId = rowid;
            BondStatusForValidation = rowData['BondStatus'];
            bondId = selectedBondId;
            selectedBondStatus = rowData['BondStatusHidden'];
            if (iCol == 8 && (window.empStatusId != 2) && rowid != "new_row") {
                DeleteBondDetails(selectedBondId);
            } else {
                if (window.empStatusId != 2) {
                    EditBondDetails(rowData);
                }
            }
            return true;
        }
    }).navGrid("#jqTableBondDetailsPager",
          { search: false, refresh: false, add: false, edit: false, del: false }
      );

   // $("#jqBondDetailsTable").jqGrid('inlineNav', "#jqTableBondDetailsPager",
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
   //        //oneditfunc: function (data, value) {
   //        oneditfunc: function (rowId) {
   //            var rowData = $("#jqBondDetailsTable").getRowData(rowId);
   //            var bstatus = rowData["BondStatusHidden"];
   //            if (bstatus == "No") {
   //                $("#new_row_ReservationNumber").val("");
   //                $("#" + rowId + "_BondAmount").addClass("bg-gray");
   //                $("#" + rowId + "_BondAmount").attr("disabled", "disabled");

   //                $("#" + rowId + "_BondOverDate").addClass("bg-gray");
   //                $("#" + rowId + "_BondOverDate").attr("disabled", "disabled");
   //            }
   //            else {
   //                $("#" + rowId + "_BondAmount").removeClass("bg-gray");
   //                $("#" + rowId + "_BondAmount").removeAttr("disabled");

   //                $("#" + rowId + "_BondOverDate").removeClass("bg-gray");
   //                $("#" + rowId + "_BondOverDate").removeAttr("disabled");
   //            }
   //        },
   //        sucessfunc: function (data) {
   //        },
   //        url: null,
   //        extraparam: {
   //            EmployeeId: function () {
   //                return bondEmployeeId
   //            },
   //            BondId: function () {
   //                return bondId
   //            },
   //            BondStatus: function () {
   //                return $('#BondStatus').val();
   //            },
   //            BondTypeId: function () {
   //                return $('#BondTypeID').val();
   //            }
   //        },
   //        aftersavefunc: function (data, response) {
   //            var result = $.parseJSON(response.responseText);
   //            if (result.status == true) {
   //                selectedRowId = 0;
   //                $("#jqBondDetailsTable").trigger("reloadGrid");
   //                //RefreshTotalExperienceView();
   //                $("#successBondDialog").dialog({
   //                    resizable: false,
   //                    height: 140,
   //                    modal: true,
   //                    title: 'Bond Details',
   //                    dialogClass: "noclose",
   //                    buttons: {
   //                        Ok: function () {
   //                            $(this).dialog("close");
   //                        }
   //                    }
   //                });
   //            }
   //            else {
   //                $("#errorBondDialog").dialog({
   //                    resizable: false,
   //                    height: 140,
   //                    modal: true,
   //                    dialogClass: "noclose",
   //                    title: 'Bond Details',
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
   //})

    if ($('#UserRole').val() != window.hrAdmin) {
        $("#jqTableBondDetailsPager_left").css("visibility", "hidden");
        $("#jqBondDetailsTable").jqGrid('hideCol', 'Delete');
    }
}); // ready end

function getBondStatus(e) {
    var status = e.target[e.target.selectedIndex].text;
    $('#BondStatus').val(status);
    BondStatusForValidation = status;
    if (status == "No") {
        $("#new_row_BondAmount").val("").addClass("bg-gray").attr("disabled", "disabled");
        $("#" + selectedRowId + "_BondAmount").val("").addClass("bg-gray").attr("disabled", "disabled");

        $("#new_row_BondOverDate").val("").addClass("bg-gray").attr("disabled", "disabled");
        $("#" + selectedRowId + "_BondOverDate").val("").addClass("bg-gray").attr("disabled", "disabled");
    }
    else {
        $("#new_row_BondAmount").removeClass("bg-gray").removeAttr("disabled");
        $("#" + selectedRowId + "_BondAmount").removeClass("bg-gray").removeAttr("disabled");

        $("#new_row_BondOverDate").removeClass("bg-gray").removeAttr("disabled");
        $("#" + selectedRowId + "_BondOverDate").removeClass("bg-gray").removeAttr("disabled");
    }
}

function getBondType(e) {
    var type = e.target[e.target.selectedIndex].text;
    var ID;
    var stype;
    $.each(bondtypeList, function (index, value) {
        var empName = value.BondType.replace('  ', ' ');

        if (empName == type) {
            ID = value.BondTypeID;
            stype = value.BondType
        }
    });
    $('#BondTypeID').val(ID);
    $('#BondType').val(stype);
}

function isValidAmount(value, Colname) {
    //if (value != '') {
    var pattern = new RegExp(/^\+?[0-9]*\.?[0-9]+$/);
    var valid = pattern.test(value);

    if (BondStatusForValidation == "Yes") {
        if (!valid) {
            $("#ValidAmountDialog").dialog({
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
    else {
        return [true, ""];
    }
    //}
}

function IsBondTypeSelected(value, colname) {
    if (value == "0") {
        $("#RequiredFeildForBondType").dialog({
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

function IsBondAmountRequired(value, colname) {
    alert("hi");
    if (BondStatusForValidation == "Yes") {
        $("#RequiredFeildForBondAmount").dialog({
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
function IsBondOverDateRequired(value, colname) {
    if (BondStatusForValidation == "Yes" && value == "") {
        $("#RequiredFeildForBondDate").dialog({
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
function IsBondStatusSelected(value, colname) {
    if (value == "0") {
        $("#RequiredFeildForBondStatus").dialog({
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

function getBondTypeList(e) {
    var type = e.target[e.target.selectedIndex].text;
    var ID;
    $.each(bondtypeList, function (index, value) {
        var empName = value.BondType.replace('  ', ' ');

        if (empName == type) {
            ID = value.BondTypeID;
        }
    });
    $('#BondTypeID').val(ID);
    $('#BondType').val(type);
}

function ChangeBondOverDate2(e) {
    //var selectedDateID = e.target.id;
    //var rowID = selectedDateID.split("_");
    //var selectedEndDate = e.target.value;
    //if (rowID[0] == "new")
    //    var startDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_StartDate").val();
    //else
    //    var startDateValue = $("#" + rowID[0] + "_StartDate").val();
    ////var startDateValue = $("#"+rowID[0]+"_StartDate").val();
    //if (selectedEndDate < startDateValue && selectedEndDate != "" && startDateValue != "") {
    //    $("#MilestoneEndDateLesserDialog").dialog({
    //        modal: true,
    //        resizable: false,
    //        height: 140,
    //        width: 300,
    //        dialogClass: "noclose",
    //        buttons:
    //        {
    //            "Ok": function () {
    //                $(this).dialog("close");
    //                if (rowID[0] == "new")
    //                    var endDateValue = $("#" + rowID[0] + "_" + rowID[1] + "_EndDate").val(oldEndDate);
    //                else
    //                    var endDateValue = $("#" + rowID[0] + "_EndDate").val(oldEndDate);
    //            }
    //        }
    //    });
    //}
}

function getBondStatusList45(e) {
    var status = e.target[e.target.selectedIndex].text;
    $('#BondStatus').val(status);
}

function SaveBondDetails2() {
    //    $("#addExpenseSuccessMessege").dialog({
    //        height: 140,
    //        width: 300,
    //        modal: true,
    //        title: "Bond Details",
    //        open: function () {
    //            //setTimeout("$('#addExpenseSuccessMessege').dialog('close')", 1000);
    //            setTimeout(function () { $("#addExpenseSuccessMessege").dialog("destroy") }, 1000);
    //        },
    //        close: function () {
    //            $("#addExpenseSuccessMessege").dialog("destroy");
    //        }
    //    });

    //jQuery("#jqBondDetailsTable").trigger("reloadGrid");

    $("#successBondDialog").dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        height: 140,
        width: 300,
        title: "Bond Details",
        dialogClass: "noclose",
        buttons: {
            "Ok": function () {
                jQuery("#jqBondDetailsTable").trigger("reloadGrid");
                $(this).dialog('close');
            }
        }
    });
}

function AddBondDetails2() {
    $('#BondDetailsDialog').dialog({
        autoOpen: false,
        modal: true,
        width: '500',
        resizable: false,
        title: 'Bond Details',
        close: function () {
            $(".field-validation-error").empty();
            $('input').removeClass("input-validation-error");
        }
    });

    $('#BondDetailsDialog #BondId').val('');
    $('#BondDetailsDialog #EmployeeId').val($("#EmployeeId").val());
    $('#BondDetailsDialog #ddlBondType').val('');
    $('#BondDetailsDialog #ddlBondStatus').val('');
    $('#BondDetailsDialog #txtBondAmount').val('');
    $('#BondDetailsDialog #txtBondDetailsOverDate').val('');
    $('#BondDetailsDialog #hdnBondType').val('');
    $('#BondDetailsDialog #hdnBondStatus').val('');
    $('#BondDetailsDialog #hdnBondAmount').val('');
    $('#BondDetailsDialog #hdnBondOverDate').val('');

    $('#BondDetailsDialog').dialog('open');
}

function EditBondDetails(object) {
    $('#BondDetailsDialog').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: 500,
        height: 'auto',
        title: "Bond Details",
        close: function () {
            $(".field-validation-error").empty();
            $('input').removeClass("input-validation-error");
        }
    });

    $('#BondDetailsDialog #BondId').val(object['BondId']);
    $("#BondDetailsDialog #ddlBondType option[value = " + object['BondTypeID'] + "] ").attr('selected', 'selected');
    $("#BondDetailsDialog #ddlBondStatus option[value = " + object['BondStatus'] + "] ").attr('selected', 'selected');
    $('#BondDetailsDialog #txtBondAmount').val(object['BondAmount']);
    $('#BondDetailsDialog #txtBondDetailsOverDate').val(object['BondOverDate']);
    $('#BondDetailsDialog #hdnBondType').val(object['BondTypeID']);
    $('#BondDetailsDialog #hdnBondStatus').val(object['BondStatus']);
    $('#BondDetailsDialog #hdnBondAmount').val(object['BondAmount']);
    $('#BondDetailsDialog #hdnBondOverDate').val(object['BondOverDate']);

    if ($("#ddlBondStatus option:selected").val() == window.yesValue) {
        $("#txtBondAmount").attr('disabled', false);
        $(".ui-datepicker-trigger").show();
        $("#BAmountMandSign").show();
        $("#BOverDateMandSign").show();
        $("#txtBondDetailsOverDate").attr('disabled', false);
    }
    if ($("#ddlBondStatus option:selected").val() == window.noValue) {
        $("#txtBondAmount").attr('disabled', true);
        //  $("#txtBondAmount").clear;
        $("#txtBondAmount").val('');
        //$("#txtBondAmount").datepicker({ dateFormat: "d-M-yy", changeMonth: true, changeYear: true, yearRange: "-20:+30",buttonImageOnly: false,buttonImage: ""});
        $(".ui-datepicker-trigger").hide();
        $("#BAmountMandSign").hide();
        $("#BOverDateMandSign").hide();
        $("#txtBondDetailsOverDate").attr('disabled', true);
        //   $("#txtBondDetailsOverDate").clear;
        $("#txtBondDetailsOverDate").val('');
        $(".field-validation-error").empty();
        $('input').removeClass("input-validation-error");
    }
    $('#BondDetailsDialog').dialog('open');
}

function DeleteBondDetails(selectedBondId) {
    $('#deleteBondDialogConfirmation').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        height: 140,
        width: 300,
        title: "Delete Bond Detail",
        dialogClass: "noclose",
        buttons:
        {
            "Ok": function () {
                $.ajax({
                    url: window.deleteBondUrl,
                    data: { employeeBondID: selectedBondId },
                    success: function (data) {
                        $("#deleteBondDialogConfirmation").dialog("close");
                        $("#deleteBondDialogConfirmation").dialog("destroy");
                        $("#deleteBondRecord").dialog({
                            modal: true,
                            resizable: false,
                            height: 140,
                            width: 300,
                            title: "Deleted",
                            dialogClass: "noclose",
                            buttons:
                            {
                                "Ok": function () {
                                    jQuery("#jqBondDetailsTable").trigger("reloadGrid");
                                    $(this).dialog('close');
                                }
                            }
                        });
                    }
                });
            },
            "Cancel": function () {
                $(this).dialog('close');
            }
        }
    });
    $('#deleteBondDialogConfirmation').dialog('open');
}