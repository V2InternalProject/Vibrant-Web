/* File Created: August 13, 2013 */
function DisplayLoadingDialog() {
    $("#loading").dialog({
        closeOnEscape: false,
        resizable: false,
        height: 140,
        width: 300,
        modal: true,
        dialogClass: "noclose",
        open: function () {
            $('#loading').parent().css('background-color', 'transparent');
            $(this).parent().prev('.ui-widget-overlay').css('z-index', '32');
            $(this).parent().css('z-index', '33');
        }
    });
}
var noRoleFlag = false;
function EmployeeDetailfunction() {
    //$(".dll option").eq(0).before($("<option></option>").val("").text("Select"));

    if (window.UserRole == 'HR Admin' || window.UserRole == 'RMG' || window.UserRole == 'HR Executive') {
        $("#frmSearchEmployeeLayout").show();
    } else {
        $("#frmSearchEmployeeLayout").hide();
    }

    $('#ParentDU').rules("add", {
        required: function () {
            return (window.UserRole == "RMG");
        },
        messages:
			{
			    required: "Parent DU is required."
			}
    });

    $('#CurrentDU').rules("add", {
        required: function () {
            return (window.UserRole == "RMG");
        },
        messages:
			{
			    required: "Current DU is required."
			}
    });

    $('#ReportingToId_Emp').rules("add", {
        required: function () {
            return (window.UserRole == "RMG");
        },
        messages:
			{
			    required: "Reporting To is required."
			}
    });

    $('#CompetencyManagerId_Emp').rules("add", {
        required: function () {
            return (window.UserRole == "RMG");
        },
        messages:
			{
			    required: "Competency Manager is required."
			}
    });

    $('#ExitConfirmationManagerId_Emp').rules("add", {
        required: function () {
            return (window.UserRole == "RMG");
        },
        messages:
			{
			    required: "Confirmation / Exit Process Manager is required."
			}
    });

    $('#Shift').rules("add", {
        required: function () {
            return (window.UserRole == "RMG");
        },
        messages:
			{
			    required: "Shift is required."
			}
    });
    $('#Group').rules("add", {
        required: function () {
            return (window.UserRole == "RMG");
        },
        messages:
			{
			    required: "Business Group is required."
			}
    });
    $('#OrganizationUnit').rules("add", {
        required: function () {
            return (window.UserRole == "RMG");
        },
        messages:
			{
			    required: "Organization Unit is required."
			}
    });

    $("#LoginRole").dropdownchecklist({ maxDropHeight: 220, width: 200, icon: {}, emptytext: "Select/View Roles" });

    if (window.EmpStatusMasterID == 2) {
        $('#ddcl-LoginRole').unbind("click");
        jQuery('form#EmployeeForm').find('input,select,textarea').attr('disabled', true);
        $("#EmployeeStatusMaster").attr("disabled", false);
        $("#RejoinedWithingOneYear").attr("disabled", false);
        $("#ContractEmployeeChkLabel").hide();
        $("#ContractEmployeeLabel").html("").append("No").show();
        $("#btnSave").hide();
        $("#btnReset").hide();
    }

    $('#JoiningDate').datepicker({
        dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, yearRange: '2000:c', maxDate: new Date, showOn: "both",
        buttonImage: "../../Images/New Design/calender-icon.png", buttonImageOnly: true, beforeShow: BeforeShowForDatePicker, onClose: onCloseForDatePicker
    })
        .bind("change", function () {
            var minValue = $(this).val();
            minValue = $.datepicker.parseDate("mm/dd/yy", minValue);
            $("#ExitDate").datepicker("option", "minDate", minValue);
            minValue.setDate(minValue.getDate() + 1);
            $('#ProbationReviewDate').datepicker("option", "minDate", minValue);
            $('#ConfirmationDate').datepicker("option", "minDate", minValue);
        });

    $('#ExitDate').datepicker({
        dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, yearRange: '2000:2050', showOn: "both", buttonImage: "../../Images/New Design/calender-icon.png",
        buttonImageOnly: true, beforeShow: BeforeShowForDatePicker, onClose: onCloseForDatePicker
    });

    $('#ProbationReviewDate').datepicker({
        dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, yearRange: "2000:2050", showOn: "both", buttonImage: "../../Images/New Design/calender-icon.png",
        buttonImageOnly: true, beforeShow: BeforeShowForDatePicker, onClose: onCloseForDatePicker
    })
        .bind("change", function () {
            var minValue = $(this).val();
            minValue = $.datepicker.parseDate("mm/dd/yy", minValue);
            minValue.setDate(minValue.getDate() + 1);
            $("#ConfirmationDate").datepicker("option", "minDate", minValue);
        });

    $('#ConfirmationDate').datepicker({ dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, yearRange: "2000:c", maxDate: new Date, showOn: "both", buttonImage: "../../Images/New Design/calender-icon.png", buttonImageOnly: true, beforeShow: BeforeShowForDatePicker, onClose: onCloseForDatePicker });

    $("#txtMonths").bind("change", function () {
        var months = parseInt($("#txtMonths").val());
        var s = $("#JoiningDate").val();
        var d = $.datepicker.parseDate("mm/dd/yy", s);
        d.setMonth(d.getMonth() + months);
        $("#ProbationReviewDate").datepicker('setDate', d);
    });

    $("#DT").change(function () {
        if ($(this).val() == true) {
            $("#ParentDU").attr("style", "visiblity: visible");
            $("#CurrentDU").attr("style", "visiblity: visible");
        } else {
            $("#ParentDU").attr("style", "visiblity: hidden");
            $("#CurrentDU").attr("style", "visiblity: hidden");
        }
    });

    $("#EmployeeStatusMaster").change(function () {
        var url = window.GetEmployeeurl;
        $("#EmployeeStatus").attr("disabled", false);
        $("#btnSave").attr("disabled", false);
        $("#btnSave").show();
        $("#btnReset").attr("disabled", false);
        $("#btnReset").show();
        //fire off the request, passing it the MatserId which is the employeementStatus selected item value
        $.getJSON(url, { MatserId: $("#EmployeeStatusMaster").val() }, function (data) {
            //Clear the Model list
            $("#EmployeeStatus").empty();
            $("#EmployeeStatus").append("<option value='" + "" + "'>" + "Select" + "</option>");
            //Foreach Model IncomeTaxNo the list, add a model option from the data returned
            $.each(data, function (index, optionData) {
                $("#EmployeeStatus").append("<option value='" + optionData.EmployeeStatusId + "'>" + optionData.EmployeeStatus + "</option>");
            });
        });
    });

    //    $("#EmployeeForm #RecruiterName").autocomplete({
    //        source: function (request, response) {
    //            $.getJSON(window.autosugest, { term: request.term }, function (data) {
    //                response($.map(data, function (el) {
    //                    var emplyeeInformation = el.EmployeeName;
    //                    return {
    //                        label: emplyeeInformation,
    //                        value: el.EmployeeName
    //                    };

    //                }));
    //            });
    //        }
    //    });

    $("#HdLoginRolesList").hide();
    if ($('#UserRole').val() != window.HRAdmin) {
        $(".ui-datepicker-trigger").hide();
        $("#ddcl-LoginRole-ddw input:checkbox").each(function () {
            var counter = 0;
            var currentVal = $(this).val();
            $(RoleSelected).each(function (index, value) {
                if (value == currentVal) {
                    counter = 1;
                }
            });
            if (counter == 1) {
                $(this).attr("checked", true);
            }
        });
    }

    if ($('#UserRole').val() != window.HRAdmin) {
        $('#EmployeeForm').find('input,select,textarea').attr('disabled', 'disabled');
        $('#lastYearAppraisalDiv').hide();
        $('#lastYearPromotionDiv').hide();
        $('#lastYearIncrementDiv').hide();
        $('#ddcl-LoginRole').unbind("click");
        $("#btnSave").hide();
        $("#btnReset").hide();
        // $('#IncomeTaxNo').attr('disabled', false);
    }
    else {
        $('#Group').attr('disabled', 'disabled');
        $('#OrganizationUnit').attr('disabled', 'disabled');
        $('#ParentDU').attr('disabled', 'disabled');
        $('#CurrentDU').attr('disabled', 'disabled');
        $('#DT').attr('disabled', 'disabled');
        $('#ResourcePoolName').attr('disabled', 'disabled');
        $('#ReportingToId_Emp').attr('disabled', 'disabled');
        $('#CompetencyManagerId_Emp').attr('disabled', 'disabled');
        $('#ExitConfirmationManagerId_Emp').attr('disabled', 'disabled');
        $('#BillableStatus').attr('disabled', 'disabled');
        $('#Shift').attr('disabled', 'disabled');
        $('#IncomeTaxNo').attr('disabled', false);
    }

    if ($('#UserRole').val() == window.RMGRole) {
        $('#ReportingToId_Emp').attr('disabled', false);
        $('#CompetencyManagerId_Emp').attr('disabled', false);
        $('#ExitConfirmationManagerId_Emp').attr('disabled', false);
        $('#btnSave').attr('disabled', false);
        $('#btnSave').show();
        $('#btnReset').attr('disabled', false);
        $('#btnReset').show();
        $('#Group').attr('disabled', false);
        $('#OrganizationUnit').attr('disabled', false);
        $('#ResourcePoolName').attr('disabled', false);
        $('#DT').attr('disabled', false);
        $('#CurrentDU').attr('disabled', false);
        $('#ParentDU').attr('disabled', false);
        $('#BillableStatus').attr('disabled', false);
        $('#Shift').attr('disabled', false);
        $('#IncomeTaxNo').attr('disabled', false);
    }

    var minValue1 = $('#JoiningDate').val();
    if (minValue1 == null) {
        minValue1 = $.datepicker.parseDate("mm/dd/yy", minValue1);
        if ($("#ExitDate").val() == "") {
            $("#ExitDate").datepicker("option", "minDate", minValue1);
        }
        minValue1.setDate(minValue1.getDate() + 1);
        if ($("#ProbationReviewDate").val() == "") {
            $('#ProbationReviewDate').datepicker("option", "minDate", minValue1);
            minValue1.setDate(minValue1.getDate() + 1);
            $('#ConfirmationDate').datepicker("option", "minDate", minValue1);
        } else {
            var probatndt = $('#ProbationReviewDate').val();
            probatndt = $.datepicker.parseDate("mm/dd/yy", probatndt);
            probatndt.setDate(probatndt.getDate() + 1);
            $('#ConfirmationDate').datepicker("option", "minDate", probatndt);
        }
    }

    $('#btnSave').click(buttonSaveFunction);
    $("#ddcl-LoginRole-ddw input:checkbox").each(function () {
        var counter = 0;
        var currentVal = $(this).val();
        $(RoleSelected).each(function (index, value) {
            if (value == currentVal) {
                counter = 1;
            }
        });

        if (counter == 1) {
            $(this).attr("checked", true);
            //$(this).attr("checked", "checked");
        }
    });

    if ($("#RejoinedWithingOneYear").is(":disabled")) {
        if ($("#RejoinedWithingOneYear").prop('checked') == false) {
            $("#ContractEmployeeChkLabel").hide();
            $("#ContractEmployeeLabel").html("").append("No").show();
        }
        else {
            $("#ContractEmployeeChkLabel").hide();
            $("#ContractEmployeeLabel").html("").append("Yes").show();
        }
    }

    if ($("#BillableStatus").is(":disabled")) {
        if ($("#BillableStatus").prop('checked') == false) {
            $("#BillableStatusChkLabel").hide();
            $("#BillableStatusLabel").html("").append("No").show();
        }
        else {
            $("#BillableStatusChkLabel").hide();
            $("#BillableStatusLabel").html("").append("Yes").show();
        }
    }
}

$("#ConfirmationDate,#ExitDate,#ProbationReviewDate").keypress(function (e) {
    if (e.keyCode == 8 || e.keyCode == 46) {
        //e.preventDefault();
        return true;
    } else {
        e.preventDefault();
        return false;
    }
});

var RestoreValues = function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    var employeecode = $('#employeecode').val();
    var exitdate = $('#exitdate').val();
    var confirmationdate = $('#confirmationdate').val();
    var employeementStatus = $('#employeestatusMaster').val();
    var employeestatus = $('#employeestatus').val();
    var appraisal = $('#appraisal').val();
    var organizationUnit = $('#organizationUnit').val();
    var officelocation = $('#officeLocation').val();

    var group = $('#group').val();
    var dt = $('#dt').val();
    var recruiterName = $('#recruiterName').val();
    var joiningdate = $('#joiningdate').val();
    var probationreviewdate = $('#probationreviewdate').val();
    var status = $('#status').val();
    var promotion = $('#promotion').val();
    var increment = $('#increment').val();
    var region = $('#region').val();
    var resourcePoolName = $('#resourcePoolName').val();
    var parentDU = $('#parentDU').val();
    var currentDU = $('#currentDU').val();
    var commitmentsMade = $('#commitmentsMade').val();
    var calender = $('#calenderName').val();
    var loginrole = $('#loginRole').val();
    var shift = $('#shift').val();
    var reportingTo = $('#ReportingToId').val();
    var competencyManager = $('#CompetencyManagerId').val();
    var exitConfirmationManager = $('#ExitConfirmationManagerId').val();
    var month = $('#months').val();
    var RejoinedWithinyear = $("#rejoinedwithingoneyear").val();

    $('#ExitConfirmationManagerId_Emp').val(exitConfirmationManager);
    $('#CompetencyManagerId_Emp').val(competencyManager);
    $('#ReportingToId_Emp').val(reportingTo);
    $('#CommitmentsMade').val(commitmentsMade);
    $('#CurrentDU').val(currentDU);
    $('#ParentDU').val(parentDU);
    $('#ResourcePoolName').val(resourcePoolName);
    $("#Months").val(month);
    $('#Region').val(region);
    $('#OfficeLocation').val(officelocation);
    $('#LastYearIncrement').val(increment);
    $('#LastYearPromotion').val(promotion);
    $('#Status').val(status);
    $('#ProbationReviewDate').val(probationreviewdate);
    $('#JoiningDate').val(joiningdate);
    $('#RecruiterName').val(recruiterName);
    $("#IncomeTaxNo").val($("#incometaxno").val());
    $('#DT').val(dt);
    $('#Group').val(group);
    $('#OrganizationUnit').val(organizationUnit);
    $('#LastYearAppraisal').val(appraisal);
    $('#EmployeeStatusMaster').val(employeementStatus);
    $('#ConfirmationDate').val(confirmationdate);
    $('#ExitDate').val(exitdate);
    $('#EmployeeCode').val(employeecode);
    $('#CalenderName').val(calender);
    $('#LoginRole').val(loginrole);
    $('#Shift').val(shift);

    if (RejoinedWithinyear == "True") {
        $("#RejoinedWithingOneYear").attr("Checked", "Checked");
        $("#ContractEmployeeChkLabel").hide();
        $("#ContractEmployeeLabel").html("").append("Yes").show();
    }
    else {
        $("#RejoinedWithingOneYear").attr("Checked", false);
        $("#ContractEmployeeChkLabel").hide();
        $("#ContractEmployeeLabel").html("").append("No").show();
    }
    //$("#RejoinedWithingOneYear").val(RejoinedWithinyear);

    //fire off the request, passing it the MatserId which is the employeementStatus selected item value
    $.getJSON(GetEmployeeurl, { MatserId: $("#EmployeeStatusMaster").val() }, function (data) {
        //Clear the Model list
        $("#EmployeeStatus").empty();
        $("#EmployeeStatus").append("<option value='" + "" + "'>" + "Select" + "</option>");
        //Foreach Model in the list, add a model option from the data returned
        $.each(data, function (index, optionData) {
            $("#EmployeeStatus").append("<option value='" + optionData.EmployeeStatusId + "'>" + optionData.EmployeeStatus + "</option>");
        });

        $("#EmployeeStatus option[value='" + employeestatus + "']").attr("selected", "selected");
    });

    $("#ddcl-LoginRole-ddw input:checkbox").each(function () {
        $(this).attr("checked", false);
    });

    $("#ddcl-LoginRole-ddw input:checkbox").each(function () {
        var counter = 0;
        var currentVal = $(this).val();
        $(RoleSelected).each(function (index, value) {
            if (value == currentVal) {
                counter = 1;
            }
        });

        if (counter == 1) {
            $(this).attr("checked", true);
        }
    });
};

function CheckLoginRole() {
    var counter = 0;
    $("#ddcl-LoginRole-ddw input:checkbox").each(function () {
        if ($(this).attr("checked")) {
            counter = 1;
        }
    });
    if (counter == 0 && $('#UserRole').val() == window.HRAdmin) {
        $("#LoginRoleddlError").dialog({
            resizable: false,
            height: 140,
            width: 300,
            modal: true,
            title: 'Employee Details',
            dialogClass: "noclose",
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                }
            }
        });
        noRoleFlag = true;
    }
    else {
        var value = "";
        $("#ddcl-LoginRole-ddw input:checkbox").each(function () {
            if ($(this).is(":checked")) {
                value += $(this).val() + ",";
            }
        });

        $("#LoginRole").val(value);
        $("#hdnSelectedRoleList").val(value);
        $("#HdLoginRolesList").val(value);
        noRoleFlag = false;
    }
}

function buttonSaveFunction() {
    CheckLoginRole();
    if (noRoleFlag == true) {
        return false;
    }
    if ($("#EmployeeForm").valid()) {
        jQuery('form#EmployeeForm').find('input,select,textarea').attr('disabled', false);
        DisplayLoadingDialog();  //checked
        $.ajax({
            url: window.postUrl,
            type: 'POST',
            data: $('#EmployeeForm').serialize(),
            success: function (results) {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");

                if ($("#EmployeeStatusMaster").val() == 3 || $("#EmployeeStatusMaster").val() == 4) {
                    $('form#EmployeeForm').find('input,select,textarea').attr('disabled', true);
                    $("#EmployeeStatusMaster").attr("disabled", false);
                    window.location.reload();
                }

                if (results.status) {
                    $("#EmployeesuccessDialog").dialog({
                        closeOnEscape: false,
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                location.reload();
                                $(this).dialog("close");
                            }
                        },
                        close: function () {
                            location.reload();
                            $(this).dialog("destroy");
                        }
                    });
                }
                else {
                    $("#errorDialog").dialog({
                        closeOnEscape: false,
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                location.reload();
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            }
        });
    }
    return false;
}

$("#RejoinedWithingOneYear").click(function () {
    if ($('#RejoinedWithingOneYear').is(':checked')) {
        $('#ConfirmationDate').val($('#JoiningDate').val());

        $("#EmployeeStatus option").each(function () {
            if ($(this).text() == 'Confirmed') {
                $(this).attr('selected', 'selected');
            }
        });
    }
    else {
        $('#ConfirmationDate').val(ConfirmationDate);
        $("#EmployeeStatus option[value = " + $('#employeestatus').val() + "] ").attr('selected', 'selected');
    }
});