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
$('#btnAddEmployeeQualifications').click(function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    //    $("#addQualificationsDialog #NewEmployeeQualification_EmployeeID").val($('#EmployeeID').val());
    $("#addQualificationsDialog #NewEmployeeQualification_EmployeeQualificationID").val('');
    $("#addQualificationsDialog #NewEmployeeQualification_Qualification").val('');
    $("#addQualificationsDialog #NewEmployeeQualification_Degree").val('');
    $("#addQualificationsDialog #NewEmployeeQualification_Type").val('');
    $("#addQualificationsDialog #NewEmployeeQualification_Course").val('');
    $("#addQualificationsDialog #NewEmployeeQualification_Institute").val('');
    $("#addQualificationsDialog #NewEmployeeQualification_Percentage").val('');
    $("#addQualificationsDialog #NewEmployeeQualification_Specialization").val('');
    $("#addQualificationsDialog #NewEmployeeQualification_University").val('');
    $("#addQualificationsDialog #NewEmployeeQualification_Year").val('');
    //    $("#addQualificationsDialog #course").val('');
    $("#addQualificationsDialog #institute").val('');
    $("#addQualificationsDialog #percentage").val('');
    $("#addQualificationsDialog #qualification").val('');
    $("#addQualificationsDialog #degree").val('');
    $("#addQualificationsDialog #type").val('');
    $("#addQualificationsDialog #specialization").val('');
    $("#addQualificationsDialog #university").val('');
    $("#addQualificationsDialog #year").val('');
    $('#spanYear').hide();
    $('#addQualificationsDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Qualification Details",
        open: function (event, ui) {
            $("#NewEmployeeQualification_Year").empty();
            filldropdownlist();
        }
    });
    $('#addQualificationsDialog').dialog('open');
});

var DeleteQualificationDetail = function (selectedQualId, employeeId) {
    $('#deleteQualificationDialogConfirmation').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        height: 'auto',
        width: 300,
        dialogClass: "noclose",
        title: "Delete Qualification Detail",
        buttons: {
            "Ok": function () {
                $.ajax({
                    url: "DeleteQualificationDetails/PersonalDetails",
                    cache: false,
                    data: { employeeQualificationID: selectedQualId, employeeId: employeeId },
                    success: function (data) {
                        if (data.status == true) {
                            $("#deleteQualificationDialogConfirmation").dialog("close");
                            $("#deleteQualificationDialogConfirmation").dialog("destroy");

                            $("#deleteQualificationRecord").dialog({
                                modal: true,
                                resizable: false,
                                height: 140,
                                width: 300,
                                title: "Deleted",
                                dialogClass: "noclose",
                                buttons:
                                    {
                                        "Ok": function () {
                                            jQuery("#employeeTable").trigger("reloadGrid");
                                            $(this).dialog('close');
                                        }
                                    }
                            });
                        } else if (data.status == "Error") {
                            $("#deleteQualificationDialogConfirmation").dialog("close");
                            $("#deleteQualificationDialogConfirmation").dialog("destroy");
                            $("#errorDialog").dialog({
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                title: 'Qualification Details',
                                dialogClass: "noclose",
                                buttons: {
                                    Ok: function () {
                                        jQuery("#employeeTable").trigger("reloadGrid");
                                        $(this).dialog("close");
                                    }
                                }
                            }); //end dialog
                        } else {
                            $("#deleteQualificationDialogConfirmation").dialog("close");
                            $("#deleteQualificationDialogConfirmation").dialog("destroy");
                            $("#errorDialog").dialog({
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                title: 'Qualification Details',
                                dialogClass: "noclose",
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                        jQuery("#employeeTable").trigger("reloadGrid");
                                    }
                                }
                            }); //end dialog
                        }
                    }
                });
            },
            "Cancel": function () {
                $(this).dialog('close');
            }
        }
    });
    $('#deleteQualificationDialogConfirmation').dialog('open');
};

var EditQualificationDetails = function (object) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $('#addQualificationsDialog').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: 500,
        height: 'auto',
        title: "Qualification Details"
    });
    $('#addQualificationsDialog').dialog('open');
    $("#addQualificationsDialog #NewEmployeeQualification_EmployeeQualificationID").val(object['EmployeeQualificationID']);
    $("#addQualificationsDialog #NewEmployeeQualification_Qualification option[value = " + object['QualificationID'] + "] ").attr('selected', 'selected');
    $("#addQualificationsDialog #NewEmployeeQualification_Degree option[value = " + object['DegreeID'] + "] ").attr('selected', 'selected');
    $("#addQualificationsDialog #NewEmployeeQualification_Type option[value = " + object['TypeID'] + "] ").attr('selected', 'selected');
    $(".ui-dialog #NewEmployeeQualification_Year option[value = " + object.Year + "] ").attr('selected', 'selected');
    //    $("#addQualificationsDialog #NewEmployeeQualification_EmployeeID").val($('#EmployeeID').val());
    //    $("#addQualificationsDialog #NewEmployeeQualification_Course").val(object['Course']);
    $("#addQualificationsDialog #NewEmployeeQualification_Institute").val(object['Institute']);
    $("#addQualificationsDialog #NewEmployeeQualification_Percentage").val(object['Percentage']);
    $("#addQualificationsDialog #NewEmployeeQualification_Specialization").val(object['Specialization']);
    $("#addQualificationsDialog #NewEmployeeQualification_University").val(object['University']);
    //    $("#addQualificationsDialog #course").val(object['Course']);
    $("#addQualificationsDialog #institute").val(object['Institute']);
    $("#addQualificationsDialog #percentage").val(object['Percentage']);
    $("#addQualificationsDialog #qualification").val(object['QualificationID']);
    $("#addQualificationsDialog #degree").val(object['DegreeID']);
    $("#addQualificationsDialog #type").val(object['TypeID']);
    $("#addQualificationsDialog #specialization").val(object['Specialization']);
    $("#addQualificationsDialog #university").val(object['University']);
    $("#addQualificationsDialog #year").val(object['Year']);
    $('#spanYear').hide();
};

var emptyDialog = function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    //    $("#NewEmployeeQualification_Course").val($('#course').val());
    $("#NewEmployeeQualification_Institute").val($('#institute').val());
    $("#NewEmployeeQualification_Percentage").val($('#percentage').val());
    $("#NewEmployeeQualification_Specialization").val($('#specialization').val());
    $("#NewEmployeeQualification_University").val($('#university').val());
    $("#NewEmployeeQualification_Year").val($('#year').val());
    $("#addQualificationsDialog #NewEmployeeQualification_Qualification option[value = " + $('#qualification').val() + "] ").attr('selected', 'selected');
    $("#addQualificationsDialog #NewEmployeeQualification_Degree option[value = " + $('#degree').val() + "] ").attr('selected', 'selected');
    $("#addQualificationsDialog #NewEmployeeQualification_Type option[value = " + $('#type').val() + "] ").attr('selected', 'selected');
}

function CallSendMailforQualification(qualificationDecryptedemployeeId) {
    DisplayLoadingDialog();//checked
    $.ajax({
        url: "MailSend/PersonalDetails",
        type: 'POST',
        async: false,
        data: { employeeId: qualificationDecryptedemployeeId, Module: "Qualification Details" },
        success: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            //            $("#AddQualificationSuccessMessege").dialog('destroy');
            if (data.validCcId == true && data.validtoId == true) {
                if (data.status == true) {
                    $("#mailSendSuccess").dialog({
                        closeOnEscape: false,
                        resizable: false,
                        height: 'auto',
                        title: "Mail Sent",
                        width: 300,
                        modal: true,
                        dialogClass: 'noclose',
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                                //window.location.reload();
                            }
                        }
                    });
                }
            }
            else if (data.status == "Error") {
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Mail Error',
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                }); //end dialog
            }
            else if (data.status == "ErrorRecipient") {
                $("#failedRecipient #span_failedRecipient").append(data.failedRecipient);
                $("#failedRecipient").dialog({
                    closeOnEscape: false,
                    resizable: false,
                    height: 'auto',
                    width: 300,
                    modal: true,
                    dialogClass: 'noclose',
                    buttons: {
                        Ok: function () {
                            $(this).dialog('close');
                        }
                    }
                });
            }
            else {
                if (data.validCcId == false) {
                    $("#InvalidEmail").dialog({
                        closeOnEscape: false,
                        resizable: false,
                        height: 'auto',
                        width: 300,
                        modal: true,
                        dialogClass: 'noclose',
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                            }
                        }
                    });
                }
                if (data.validtoId == false) {
                    $("#InvalidEmail").dialog({
                        closeOnEscape: false,
                        resizable: false,
                        height: 'auto',
                        width: 300,
                        modal: true,
                        dialogClass: 'noclose',
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                            }
                        }
                    });
                }
                return false;
            }
        },
        error: function () {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#AddQualificationSuccessMessege").dialog('destroy');
            $("#mailError").dialog({
                resizable: false,
                height: 'auto',
                width: 'auto',
                modal: true,
                title: 'Mail Error',
                dialogClass: 'noclose',
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    });
}

function isQualificationSelected(value, colname) {
    if (value == "0") {
        $("#QualificationRequiredDialog").dialog({
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
}//end function

function ChangeQualification(e) {
    var qualificationName = e.target[e.target.selectedIndex].text;
    var ID = 0;
    $.each(stringQualificationList, function (index, value) {
        if (value.Qualification == qualificationName) {
            ID = value.QualificationID;
        }
    });
    $('#QualificationDetailsForm #QualificationID').val(ID);
    $('#' + SelectedEducationRowId + '_Qualification').attr('title', qualificationName);
}//end function

function isDegreeSelected(value, colname) {
    if (value == "0") {
        $("#DegreeRequiredDialog").dialog({
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
}//end function

function ChangeDegree(e) {
    var degreeName = e.target[e.target.selectedIndex].text;
    var ID = 0;
    $.each(stringDegreeList, function (index, value) {
        if (value.Degree == degreeName) {
            ID = value.DegreeID;
        }
    });
    $('#QualificationDetailsForm #DegreeID').val(ID);
    $('#' + SelectedEducationRowId + '_Degree').attr('title', degreeName);
}//end function

function isYearSelected(value, colname) {
    if (value == "0") {
        $("#YearRequiredDialog").dialog({
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
}//end function

function ChangeYear(e) {
    var yearName = e.target[e.target.selectedIndex].text;
    var ID = 0;
    $.each(stringYearList, function (index, value) {
        if (value.Year == yearName) {
            ID = value.Year;
        }
    });
    $('#QualificationDetailsForm #YearID').val(ID);
    $('#' + SelectedEducationRowId + '_Year').attr('title', yearName);
}//end function

function isTypeSelected(value, colname) {
    if (value == "0") {
        $("#TypeRequiredDialog").dialog({
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
}//end function

function ChangeType(e) {
    var typeName = e.target[e.target.selectedIndex].text;
    var ID = 0;
    $.each(stringTypeList, function (index, value) {
        if (value.Type == typeName) {
            ID = value.TypeID;
        }
    });
    $('#QualificationDetailsForm #TypeID').val(ID);
    $('#' + SelectedEducationRowId + '_Type').attr('title', typeName);
}//end function

function isValidPercentage(value, colname) {
    if (isNaN(value) == false) {
        if (value > 100 || value < 0) {
            $("#ValidPercentageDialog").dialog({
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
}//end function