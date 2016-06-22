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
 /* File Created: August 13, 2013 */
function DisplayNotPresentSpouseDetails(isApproved, isPresent, MaritalStatus, approvalStatusId) {
    if (isPresent == false && (approvalStatusId != "" && approvalStatusId != null && approvalStatusId != 1)) {
        $("#SpouseDetailsNotPresentDialogMessage").html("Employee is " + MaritalStatus);
    } else if (isApproved == false && (approvalStatusId == "" || approvalStatusId == null || approvalStatusId == 1)) {
        $("#SpouseDetailsNotPresentDialogMessage").html("Marital Status is not yet Approved by HR Admin.");
    }

    $("#SpouseDetailsNotPresentDialogMessage").dialog({
        modal: true,
        dialogClass: "noclose",
        title: 'Spouse Details',
        buttons: {
            "OK": function () {
                $("#SpouseDetailsNotPresentDialogMessage").dialog('close');
            }
        }
    });
}

function RemoveOwnTravelDetailsContainer() {
    $('#frmAddVisaDetails #ddlCountryList').val('');
    $("#ownTravelDetailsMainContainer").html('');
}

function RemoveSpouseTravelDetailsContainer() {
    $("#spouseTravelDetailsMainContainer").html('');
}

function LinkPassportClick(event, EmployeeID, personType) {
    DisplayLoadingDialog(); //checked
    var DocumentID = event.id;
    $.ajax({
        url: "showPassportDetails/EmployeeDetails",
        data: { EmployeeID: EmployeeID, DocumentID: DocumentID, PersonType: personType },
        type: 'GET',
        success: function (result) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#ShowEmployeePassportDetails").empty().append(result);
            $("#ShowEmployeePassportDetails").dialog({
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                modal: true,
                width: '300',
                dialogClass: "noclose",
                title: "Passport History",
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                        if (personType == "Own")
                            $("#EmployeePassportTable").trigger("reloadGrid");
                        else
                            $("#SpousePassportTable").trigger("reloadGrid");
                    }
                },
                close: function (event, ui) {
                    $("#ShowEmployeePassportDetails").empty();
                    $("#ShowEmployeePassportDetails").dialog("destroy");
                    if (personType == "Own")
                        $("#EmployeePassportTable").trigger("reloadGrid");
                    else
                        $("#SpousePassportTable").trigger("reloadGrid");
                }
            });
        }
    });
}

function LinkVisaClick(event, EmployeeID, personType) {
    DisplayLoadingDialog(); //checked
    var EmployeeVisaId = event.id;
    $.ajax({
        url: "ShowVisaDetails/EmployeeDetails",
        data: { EmployeeID: EmployeeID, EmployeeVisaId: EmployeeVisaId, PersonType: personType },
        type: 'GET',
        success: function (result) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#ShowEmployeeVisaDetails").empty().append(result);
            $("#ShowEmployeeVisaDetails").dialog({
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                modal: true,
                width: '300',
                title: "Visa History",
                dialogClass: "noclose",
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                        if (personType == "Own")
                            $("#jqPersonTravelTable").trigger("reloadGrid");
                        else
                            $("#spousejqTable").trigger("reloadGrid");
                    }
                },
                close: function (event, ui) {
                    $("#ShowEmployeeVisaDetails").empty();
                    $("#ShowEmployeeVisaDetails").dialog("destroy");
                    if (personType == "Own")
                        $("#jqPersonTravelTable").trigger("reloadGrid");
                    else
                        $("#spousejqTable").trigger("reloadGrid");
                }
            });
        }
    });
}

function DeletePassportDocument(DocumentID, personType) {
    if (DocumentID != 0) {
        $("#ConfirmEmployeePassportDelete").dialog({
            closeOnEscape: false,
            resizable: false,
            height: 'auto',
            width: 'auto',
            modal: true,
            dialogClass: "noclose",
            buttons: {
                "Ok": function () {
                    $(this).dialog("close");
                    DisplayLoadingDialog(); //checked
                    $.ajax({
                        url: "DeletePassportDocument/EmployeeDetails",
                        data: { DocumentID: DocumentID, PersonType: personType },
                        type: 'POST',
                        success: function (result) {
                            $("#loading").dialog("close");
                            $("#loading").dialog("destroy");
                            if (result.status == true) {
                               // $(this).dialog("close");
                                $("#DeleteEmployeePassportSuccess").dialog({
                                    closeOnEscape: false,
                                    resizable: false,
                                    height: 'auto',
                                    width: 'auto',
                                    modal: true,
                                    dialogClass: "noclose",
                                    buttons: {
                                        "Ok": function () {
                                            $(this).dialog("close");
                                            if (personType == "Own")
                                                $("#EmployeePassportTable").trigger("reloadGrid");
                                            else
                                                $("#SpousePassportTable").trigger("reloadGrid");
                                        }
                                    }
                                });
                            } //if close
                            else {
                                $("#loading").dialog("close");
                                $("#loading").dialog("destroy");
                                DisplayErrorDialog();
                            }
                        }, //sucuess end
                        error: function () {
                            DisplayErrorDialog();
                        }
                    }); //ajax close
                }, //ok
                "Cancel": function () {
                    $("#ConfirmEmployeePassportDelete").dialog("destroy");
                }
            }
        });         //close dialog
    } //end if
}