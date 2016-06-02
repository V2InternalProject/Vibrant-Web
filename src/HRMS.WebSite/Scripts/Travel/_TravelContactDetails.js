//add EmergencyContactDetails
function AddemergencyContactDetails() {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#dialogActionEmergencyContact #EmergencyDetails_id").val('');
    $("#dialogActionEmergencyContact #TravelId").val(0);
    $("#dialogActionEmergencyContact #EmployeeEmergencyContactId").val(0);
    $("#dialogActionEmergencyContact #EmployeeEmergencyContactIdNew").val(0);
    $("#dialogActionEmergencyContact #EmergencyDetails_Name").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_Address").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_ContactNo").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_EmailId").val('');
    $("#dialogActionEmergencyContact #EmergencyDetails_Relationship").val('');

    $('#dialogActionEmergencyContact').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Emergency Contact"
    });

    $('#dialogActionEmergencyContact').dialog('open');
}

//edit contact details
function EditTravelContactDetails(Object) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#dialogActionEmergencyContact #TravelId").val(Object['TravelId']);
    $("#dialogActionEmergencyContact #EmergencyDetails_Name").val(Object['Name']);
    $("#dialogActionEmergencyContact #EmployeeEmergencyContactId").val(Object['EmployeeEmergencyContactId']);
    $("#dialogActionEmergencyContact #EmployeeEmergencyContactIdNew").val(Object['EmployeeEmergencyContactId']);

    $("#dialogActionEmergencyContact #EmergencyDetails_Address").val(Object['EmgAddress']);

    $("#dialogActionEmergencyContact #EmergencyDetails_ContactNo").val(Object['ContactNo']);
    $("#dialogActionEmergencyContact #EmergencyDetails_EmailId").val(Object['EmailId']);

    $("#dialogActionEmergencyContact #EmergencyDetails_Relationship option[value = " + Object['uniqueID'] + "] ").attr('selected', 'selected');

    $('#dialogActionEmergencyContact').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Emergency Contact",
        dialogClass: "noclose"
    });

    $('#dialogActionEmergencyContact').dialog('open');
}

//delete EmergencyContactdetails
function DeleteTravelContactDetails(deleteId, travelId) {
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
                    url: "DeleteTravelContactDetails/Travel",
                    data: { contactId: deleteId, travelId: travelId },
                    success: function (data) {
                        if (data.status == true) {
                            $("#deleteEmergencyContactDialogConfirmation").dialog("close");
                            $("#deleteEmergencyContact").dialog({
                                modal: true,
                                resizable: false,
                                height: 'auto',
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