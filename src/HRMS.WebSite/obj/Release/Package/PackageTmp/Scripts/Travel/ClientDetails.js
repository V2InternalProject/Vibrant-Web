$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

//$("#addClientDetails").click(function () {
function AddClientDetails() {
    if (('@ViewBag.UserRole' == "Travel_Admin") && ('@ViewBag.StageID' == 3)) {
        $('#ClientName').removeAttr("disabled");
        $('#Compony').removeAttr("disabled");
        $('#ClientContact').removeAttr("disabled");
        $('#ClientContactNumber').removeAttr("disabled");
        $('#ClientAddress').removeAttr("disabled");
        $('#ClientEmailId').removeAttr("disabled");
        $('#PurposeOfVisit').removeAttr("disabled");
        $(".field-validation-error").empty();
        $('input').removeClass("input-validation-error");
        $("#addTravelsClientDetails #ClientId").val('');
        $("#addTravelsClientDetails #ClientName").val('');
        //$("#addTravelsClientDetails #Compony").val('');
        $("#addTravelsClientDetails #ClientContact").val('');
        $("#addTravelsClientDetails #ClientContactNumber").val('');
        $("#addTravelsClientDetails #ClientAddress").val('');
        $("#addTravelsClientDetails #ClientEmailId").val('');
        $("#addTravelsClientDetails #PurposeOfVisit").val('');
    }
    else {
        $(".field-validation-error").empty();
        $('input').removeClass("input-validation-error");
        $("#addTravelsClientDetails #ClientId").val('');
        $("#addTravelsClientDetails #ClientName").val('');
        // $("#addTravelsClientDetails #Compony").val('');
        $("#addTravelsClientDetails #ClientContact").val('');
        $("#addTravelsClientDetails #ClientContactNumber").val('');
        $("#addTravelsClientDetails #ClientAddress").val('');
        $("#addTravelsClientDetails #ClientEmailId").val('');
        $("#addTravelsClientDetails #PurposeOfVisit").val('');
    }
    $('#addClientDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Add Client Details"
    });
    $('#addClientDialog').dialog('open');
}

function LinkClientLetterClick(event, TravelID) {
    var DocumentID = event.id;
    window.location = 'DownloadClientLetteFile/Travel?DocumentID=' + DocumentID + '&TravelID=' + TravelID;
}

function LinkClientLetterClickUpload(ClitntId, TravelID, locationId) {
    $('#addTravelsClientFileDetails #ClientId').val(ClitntId);
    $('#addTravelsClientFileDetails #selectedLocationID').val(TravelID);
    $('#addTravelsClientFileDetails #TravelId').val(locationId);
    $('#addClientFileDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Edit Client Details"
    }
        );
    $('#addClientFileDialog').dialog('open');
}

function AddClientDetails() {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#FalseClientFileUploadTxt").val("No files selected");
    $("#addTravelsClientDetails #clientInviteLetter").replaceWith($("#addTravelsClientDetails #clientInviteLetter").val("").clone(true));
    $('#addClientDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Travel Journey"
    });
    $('#addClientDialog').dialog('open');
}

function resetClientDetails() {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#FalseClientFileUploadTxt").val("No files selected");
    $('#clientInviteLetter').replaceWith($('#clientInviteLetter').val("").clone(true));
}

//Edit client details
function EditClientDetails(Object) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    if (Object['TravellingLocId'] == 5) {
        $('#addTravelsClientDetails #ClientId').val(Object['ClientId']);
        $('#addTravelsClientDetails #TravellingLocId').val(Object['TravellingLocId']);
        $('#addTravelsClientDetails #PurposeOfVisit').val(Object['PurposeOfVisit']);
        $('#addTravelsClientDetails #ProspectName').val(Object['ProspectName']);
        $('#addTravelsClientDetails #ClientName').val(null);
        //$('#addTravelsClientDetails #Compony').val(null);
        $('#addTravelsClientDetails #ClientContact').val(null);
        $('#addTravelsClientDetails #ClientContactNumber').val(null);
        $('#addTravelsClientDetails #ClientAddress').val(null);
        $('#addTravelsClientDetails #ClientEmailId').val(null);
        //$('#addTravelsClientDetails #ProspectName').show();
        $("#addTravelsClientDetails #divProspectName").show();
        hideclientelement();
    }
    else if (Object['TravellingLocId'] != 4) {
        $('#addTravelsClientDetails #ClientId').val(Object['ClientId']);
        $('#addTravelsClientDetails #TravellingLocId').val(Object['TravellingLocId']);
        $('#addTravelsClientDetails #PurposeOfVisit').val(Object['PurposeOfVisit']);
        $('#addTravelsClientDetails #clientInviteLetter').val(Object['ClientInviteLetterName']);
        $('#addTravelsClientDetails #ClientName').val(null);
        //$('#addTravelsClientDetails #Compony').val(null);
        $('#addTravelsClientDetails #ClientContact').val(null);
        $('#addTravelsClientDetails #ClientContactNumber').val(null);
        $('#addTravelsClientDetails #ClientAddress').val(null);
        $('#addTravelsClientDetails #ClientEmailId').val(null);
        HideClientLocationDetails();
    }
    else {
        ShowClientLocationDetails();
        $("#addTravelsClientDetails #divProspectName").hide();
        $('#addTravelsClientDetails #ClientId').val(Object['ClientId']);
        $('#addTravelsClientDetails #ClientName').val(Object['ProjectNameId']);
        //$('#addTravelsClientDetails #Compony').val(Object['Compony']);
        $('#addTravelsClientDetails #ClientContact').val(Object['ClientContact']);
        $('#addTravelsClientDetails #ClientContactNumber').val(Object['ClientContactNumber']);
        $('#addTravelsClientDetails #ClientAddress').val(Object['ClientAddress']);
        $('#addTravelsClientDetails #ClientEmailId').val(Object['ClientEmailId']);
        $('#addTravelsClientDetails #PurposeOfVisit').val(Object['PurposeOfVisit']);
        $('#addTravelsClientDetails #TravellingLocId').val(Object['TravellingLocId']);
    }
    $('#addClientDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Edit Client Details"
    }
        );
    $('#addClientDialog').dialog('open');
}

function SaveClientDetails() {
    //    if ($('#addTravelsClientDetails').valid()) {
    //        $.ajax({
    //            url: "SaveClientDetails/Travel",
    //            async: 'true',
    //            type: 'POST',
    //            data: $('#addTravelsClientDetails').serialize(),
    $('#addTravelsClientDetails').ajaxForm({
        success: function (results) {
            var results = $.parseJSON(results);
            $('#addClientDialog').dialog("close");
            if (results.status == true) {
                $("#clientInviteLetter").replaceWith($("#clientInviteLetter").clone(true));
                //jQuery("#clientTable").trigger("reloadGrid");

                if (results.ClientId == 0)
                    $("#clientTable").find('label[id="undefined_UploadedFileName"]').text(results.FileName).show();
                else
                    $("#clientTable").find('label[id="' + results.ClientId + "_UploadedFileName" + '"]').text(results.FileName).show();
                $("#UploadClientSuccessMessege").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 300,
                    modal: true,
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            //jQuery("#clientTable").trigger("reloadGrid");
                        }
                    }
                });
            }
            else if (results.status == "Error") {
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Client Details',
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            //jQuery("#clientTable").trigger("reloadGrid");
                        }
                    }
                }); //end dialog
            }
            else {
                $("#UploadClientErrorMessege").dialog({
                    resizable: false,
                    height: 140,
                    width: 300,
                    modal: true,
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            //jQuery("#clientTable").trigger("reloadGrid");
                        }
                    }
                });
            }
        }
    });
    return true;
    //    }
    //    else
    //        return false;
}

$('#clientInviteLetter').change(function () {
    //    var file = $('input[type="file"]').val();
    var file = $(this).val();
    if (file != "") {
        $("#ClientLetterUploadError").hide();
        var path = $(this).val().replace("C:\\fakepath\\", "");
        $("#FalseClientFileUploadTxt").val(path);
    }
    var exts = ['exe'];
    // first check if file field has any value
    if (file) {
        $("#ClientLetterUploadError").text("");
        // split file name at dot
        var get_ext = file.split('.');
        // reverse name to check extension
        get_ext = get_ext.reverse();
        // check file type is valid as given in 'exts' array
        if ($.inArray(get_ext[0].toLowerCase(), exts) == 0) {
            $("#ClientInviteLetterUploadError").dialog({
                title: 'Error',
                resizable: false,
                height: 'auto',
                width: 300,
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                        if ($.browser.msie) {
                            $('#clientInviteLetter').replaceWith($('#clientInviteLetter').clone(true));
                        } else {
                            $('#clientInviteLetter').val('');
                        }
                    }
                },
                close: function () {
                    if ($.browser.msie) {
                        $('#clientInviteLetter').replaceWith($('#clientInviteLetter').clone(true));
                    } else {
                        $('#clientInviteLetter').val('');
                    }
                }
            });
        }
    }
});

function DeleteClientDetail(ClitntId) {
    $('#deleteClientDialogConfirmation').dialog(
			{
			    autoOpen: false,
			    modal: true,
			    width: 300,
			    height: 'auto',
			    resizable: false,
			    title: "Delete Client Detail",
			    dialogClass: "noclose",
			    buttons:
					{
					    Ok: function () {
					        $.ajax({
					            url: "DeleteClientDetails/Travel",
					            data: { clientId: ClitntId },
					            success: function (data) {
					                if (data.status == true) {
					                    $("#deleteClientDialogConfirmation").dialog("close");
					                    $("#deleteClientRecord").dialog({
					                        modal: true,
					                        resizable: false,
					                        height: 'auto',
					                        width: 300,
					                        title: "Deleted",
					                        dialogClass: "noclose",
					                        buttons:
					                        {
					                            "Ok": function () {
					                                $(this).dialog('close');
					                                jQuery("#clientTable").trigger("reloadGrid");
					                            }
					                        }
					                    });
					                }
					                else if (data.status == "Error") {
					                    $("#deleteClientDialogConfirmation").dialog("close");
					                    $("#errorDialog").dialog({
					                        resizable: false,
					                        height: 'auto',
					                        width: 'auto',
					                        modal: true,
					                        title: 'Dependent Details',
					                        buttons: {
					                            Ok: function () {
					                                $(this).dialog("close");
					                                jQuery("#clientTable").trigger("reloadGrid");
					                            }
					                        }
					                    }); //end dialog
					                }
					                else {
					                    $("#deleteClientDialogConfirmation").dialog("close");
					                    $("#errorDialog").dialog({
					                        resizable: false,
					                        height: 'auto',
					                        width: 'auto',
					                        modal: true,
					                        title: 'Dependent Details',
					                        buttons: {
					                            Ok: function () {
					                                $(this).dialog("close");
					                                jQuery("#clientTable").trigger("reloadGrid");
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
    $('#deleteClientDialogConfirmation').dialog('open');
}

// this function has to be removed ------------------------------------------------------- prasad
function SaveClientForm() {
    //     DisplayLoadingDialog();
    $.ajax({
        url: "SaveClientForm/Travel",
        //        async: 'true',
        type: 'POST',
        data: $('#ClientForm').serialize(),
        success: function (results) {
            if (results.status == true) {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                $("#AddClientSuccessMessege").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 300,
                    modal: true,
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            var selected = $("#tabs").tabs("option", "selected");
                            $("#tabs").tabs("option", "selected", selected + 1);
                            var postUrl3 = '@Url.Action("GetAccomodationFormDetails", "Travel")';
                            var employeeid = results.EmployeeID;
                            var traveld = results.TravelID;

                            $.ajax({
                                url: postUrl2,
                                type: 'GET',
                                data: { TravelEmployeeId: employeeid, encryptedTravelId: traveld },
                                success: function () {
                                }
                            })
                        }
                    }
                });
            }
            else if (results.status == "Error") {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Dependent Details',
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                }); //end dialog
            }
            else {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                $("#AddClientErrorMessege").dialog({
                    resizable: false,
                    height: 140,
                    width: 300,
                    modal: true,
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
        }
    });
    return true;
}