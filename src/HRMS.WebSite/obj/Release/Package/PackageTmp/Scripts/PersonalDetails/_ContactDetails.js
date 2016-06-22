//delete EmergencyContactdetails
function DeleteEmergencyContactDetail(deleteId, contactEmployeeId) {
    $('#deleteEmergencyContactDialogConfirmation').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: 300,
        height: 'auto',
        title: "Delete Emergency Contact",
        dialogClass: "noclose",
        buttons: {
            Ok: function () {
                $.ajax({
                    url: "DeleteEmployeeEmergencyContact/PersonalDetails",
                    data: { EmployeeEmergencyContactId: deleteId, contactEmployeeId: contactEmployeeId },
                    success: function (data) {
                        if (data.status == true) {
                            $("#deleteEmergencyContactDialogConfirmation").dialog("close");
                            $("#deleteEmergencyContact").dialog({
                                modal: true,
                                resizable: false,
                                height: 140,
                                width: 300,
                                title: "Deleted",
                                dialogClass: "noclose",
                                buttons: {
                                    "Ok": function () {
                                        $(this).dialog('close');
                                        jQuery("#jqEmergencyContactTable").trigger("reloadGrid");
                                    }
                                }
                            });
                        }
                        else if (data.status == "Error") {
                            $("#deleteEmergencyContactDialogConfirmation").dialog("close");
                            $("#errorDialog").dialog({
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                title: 'Contact Details',
                                dialogClass: "noclose",
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                        jQuery("#jqEmergencyContactTable").trigger("reloadGrid");
                                    }
                                }
                            }); //end dialog
                        }
                        else {
                            $("#deleteEmergencyContactDialogConfirmation").dialog("close");
                            $("#errorDialog").dialog({
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                title: 'Contact Details',
                                dialogClass: "noclose",
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                        jQuery("#jqEmergencyContactTable").trigger("reloadGrid");
                                    }
                                }
                            }); //end dialog
                        }
                    }
                });
            },
            Cancel: function () {
                $(this).dialog('close');
            }
        }
    }
    );
    $('#deleteEmergencyContactDialogConfirmation').dialog('open');
}

//edit contact details
function EditEmergencyContactDetails(Object) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#dialogActionEmergencyContact #EmployeeId").val(Object['EmployeeId']);
    $("#dialogActionEmergencyContact #EmergencyDetails_Name").val(Object['Name']);
    $("#dialogActionEmergencyContact #EmergencyDetails_id").val(Object['EmployeeEmergencyContactId']);
    $("#dialogActionEmergencyContact #EmergencyDetails_Address").val(Object['EmgAddress']);
    $("#dialogActionEmergencyContact #EmergencyDetails_ContactNo").val(Object['ContactNo']);
    $("#dialogActionEmergencyContact #EmergencyDetails_EmailId").val(Object['EmailId']);

    $("#dialogActionEmergencyContact #EmergencyDetails_Relationship option[value = " + Object['uniqueID'] + "] ").attr('selected', 'selected');

    $("#dialogActionEmergencyContact #EmergencyDetails_name").val(Object['Name']);
    $("#dialogActionEmergencyContact #EmergencyDetails_address").val(Object['EmgAddress']);
    $("#dialogActionEmergencyContact #EmergencyDetails_emailId").val(Object['EmailId']);
    $("#dialogActionEmergencyContact #EmergencyDetails_contactNo").val(Object['ContactNo']);
    $("#dialogActionEmergencyContact #EmergencyDetails_relationship").val(Object['Relation']);

    $('#dialogActionEmergencyContact').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Emergency Contact"
    });

    $('#dialogActionEmergencyContact').dialog('open');
}

//reset button
var RestoreValues = function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");

    $('#AlternateContactNumber').val($('#altContact').val());
    $('#AlternateEmailId').val($('#altEmail').val());
    $('#EmergencyContactNumber').val($('#emergencyContact').val());
    $('#GtalkId').val($('#gtalkId').val());
    $('#MobileNumber').val($('#mobNumber').val());
    $('#OfficeEmailId').val($('#officeEmail').val());
    $('#OfficeVoip').val($('#officeVoip').val());
    $('#PersonalEmailId').val($('#personalEmail').val());
    $('#ResidenceNumber').val($('#resNumber').val());
    $('#ResidenceVoip').val($('#resVoip').val());
    $('#SkypeId').val($('#skypeId').val());
    $('#YIMId').val($('#yimId').val());
    $('#SeatingLocation').val($('#seatLocation').val());
}

//add EmergencyContactDetails
function AddemergencyContactDetails() {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#dialogActionEmergencyContact #EmergencyDetails_id").val(0);
    $("#dialogActionEmergencyContact #EmergencyDetails_Name").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_Address").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_ContactNo").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_EmailId").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_Relationship").val('');

    $("#dialogActionEmergencyContact #EmergencyDetails_name").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_address").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_emailId").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_contactNo").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_relationship").val('');

    $('#dialogActionEmergencyContact').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Emergency Contact"
    });

    $('#dialogActionEmergencyContact').dialog('open');
}

//reset EmergencyContactDetails
function resetcontact() {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $('#EmergencyDetails_Name').val($('#EmergencyDetails_name').val());
    $('#EmergencyDetails_Address').val($('#EmergencyDetails_address').val());
    $('#EmergencyDetails_EmailId').val($('#EmergencyDetails_emailId').val());
    $('#EmergencyDetails_ContactNo').val($('#EmergencyDetails_contactNo').val());
    if ($('#EmergencyDetails_relationship').val() == "") {
        $("#EmergencyDetails_Relationship option[value = " + $('#EmergencyDetails_relationship').val() + "] ").attr('selected', 'selected');
    }
    else {
        $("#EmergencyDetails_Relationship option:contains(" + $('#EmergencyDetails_relationship').val() + ')').attr('selected', 'selected');
    }
}

function isRelationSelected(value, colname) {
    if (value == "0") {
        $("#EmerRelationRequiredDialog").dialog({
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

function ChangeContactRelation(e) {
    var relationName = e.target[e.target.selectedIndex].text;
    var ID = 0;
    $.each(stringRelationList, function (index, value) {
        if (value.Relation == relationName) {
            ID = value.uniqueID;
        }
    });
    $('#contactDetails #uniqueID').val(ID);
    $('#' + SelectedContactRowId + '_Relation').attr('title', relationName);
}//end function

function isValidContactEmailAddress(value, Colname) {
    var pattern = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i);

    var valid = pattern.test(value);

    if (!valid && value != "") {
        $("#ContactEmailValid").dialog({
            modal: true,
            resizable: false,
            height: 140,
            width: 300,
            title: "Deleted",
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

function isValidContactNo(value, Colname) {
    var pattern = new RegExp(/^[0-9.\+\(\)\-\s]+$/);
    var valid = pattern.test(value);

    if (!valid) {
        $("#ValidContactNoDialog").dialog({
            modal: true,
            resizable: false,
            height: 140,
            width: 300,
            title: "Deleted",
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