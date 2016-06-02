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
$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});
function AddAdminAccomodation() {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    //    $('#adminAccomodationDialog #AccomodationID').val(0);
    //    $('#adminAccomodationDialog #hotelName').val('');
    //    $('#adminAccomodationDialog #hotelAddress').val('');
    //    $('#adminAccomodationDialog #hotelContactNumber').val('');
    //    $('#adminAccomodationDialog #bookingFromDate').val('');
    //    $('#adminAccomodationDialog #bookingToDate').val('');
    //    $('#adminAccomodationDialog #roomDetails').val('');
    //    $('#adminAccomodationDialog #checkinDetails').val('');
    //    $('#adminAccomodationDialog #checkoutDetails').val('');
    //    $('#adminAccomodationDialog #additionalDetails').val('');

    //    $('#adminAccomodationDialog #HotelName').val('');
    //    $('#adminAccomodationDialog #HotelAddress').val('');
    //    $('#adminAccomodationDialog #HotelContactNumber').val('');
    //    $('#adminAccomodationDialog #BookingFromDate').val('');
    //    $('#adminAccomodationDialog #BookingToDate').val('');
    //    $('#adminAccomodationDialog #RoomDetails').val('');
    //    $('#adminAccomodationDialog #CheckinDetails').val('');
    //    $('#adminAccomodationDialog #CheckoutDetails').val('');
    //    $('#adminAccomodationDialog #AdditionalDetails').val('');
    $("#adminAccomodationDialog #FalseAccFileUploadBtn").val("No files selected");
    $('#adminAccomodationDialog #Accoupload').val('');
    $('#adminAccomodationDialog #Accoupload').replaceWith($('#adminAccomodationDialog #Accoupload').val("").clone(true));
    $('#adminAccomodationDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 490,
        resizable: false,
        title: "Add Accommodation Details"
    });
    $('#adminAccomodationDialog').dialog('open');
}

function emptyDialog() {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#HotelName").val($('#hotelName').val());
    $("#HotelAddress").val($('#hotelAddress').val());
    $("#HotelContactNumber").val($('#hotelContactNumber').val());
    $("#BookingFromDate").val($('#bookingFromDate').val());
    $("#BookingToDate").val($('#bookingToDate').val());
    $("#RoomDetails").val($('#roomDetails').val());
    $("#CheckinDetails").val($('#checkinDetails').val());
    $("#CheckoutDetails").val($('#checkoutDetails').val());
    $("#AdditionalDetails").val($('#additionalDetails').val());
    $("#FalseAccFileUploadBtn").val("No files selected");
    $('#Accoupload').replaceWith($('#Accoupload').val("").clone(true));
}

function AddAdminAccomodationDetails() {
    $('#frmAddAdminAccomodation').ajaxForm({
        success: function (results) {
            var results = $.parseJSON(results);
            if (results.status == true) {
                $("#adminAccomodationDialog").dialog("close");
                if (results.AccomodationId == 0)
                    $("#jqAccomodationAdminTabel").find('label[id="undefined_UploadedFileName"]').text(results.FileName).show();
                else
                    $("#jqAccomodationAdminTabel").find('label[id="' + results.AccomodationId + "_UploadedFileName" + '"]').text(results.FileName).show();
                $("#AccomodationUploadSuccessMessege").dialog({
                    resizable: false,
                    height: 140,
                    width: 300,
                    modal: true,
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            //jQuery("#jqAccomodationAdminTabel").trigger("reloadGrid");
                        }
                    }
                });
            }
            else if (results.status == "Error") {
                $("#adminAccomodationDialog").dialog("close");
                //                $("#AccomodationUploadErrorMessege").dialog({
                //                    resizable: false,
                //                    height: 'auto',
                //                    width: 'auto',
                //                    modal: true,
                //                    title: 'Accommodation Details',
                //                    dialogClass: "noclose",
                //                    buttons: {
                //                        Ok: function () {
                //                            $(this).dialog("close");
                //                            //jQuery("#jqAccomodationAdminTabel").trigger("reloadGrid");
                //                        }
                //                    }
                //                }); //end dialog
                DisplayErrorDialog();
            }
            else {
                $("#AddAccomodationErrorMessege").dialog({
                    resizable: false,
                    height: 140,
                    width: 300,
                    modal: true,
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            //jQuery("#jqAccomodationAdminTabel").trigger("reloadGrid");
                        }
                    }
                });
            }
        }
    });
    return true;
}

function EditAdminAccomodationDetails(object, userRole, travelAdmin) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    if (userRole != travelAdmin) {
        $(".AdminFields").attr("disabled", "disabled");
        $("#BookingFromDateCollection .ui-datepicker-trigger").hide();
        $("#BookingToDateCollection .ui-datepicker-trigger").hide();
    }
    $('#adminAccomodationDialog #AccomodationID').val(object['AccomodationID']);
    $('#adminAccomodationDialog #TravelId').val(object['TravelId']);
    $('#adminAccomodationDialog #HotelName').val(object['HotelName']);
    $('#adminAccomodationDialog #HotelAddress').val(object['HotelAddress']);
    $('#adminAccomodationDialog #HotelContactNumber').val(object['HotelContactNumber']);
    $('#adminAccomodationDialog #BookingFromDate').val(object['BookingFromDate']);
    $('#adminAccomodationDialog #BookingToDate').val(object['BookingToDate']);
    $('#adminAccomodationDialog #RoomDetails').val(object['RoomDetails']);
    $('#adminAccomodationDialog #CheckinDetails').val(object['CheckinDetails']);
    $('#adminAccomodationDialog #CheckoutDetails').val(object['CheckoutDetails']);
    $('#adminAccomodationDialog #AdditionalDetails').val(object['AdditionalDetails']);

    $('#adminAccomodationDialog #hotelName').val(object['HotelName']);
    $('#adminAccomodationDialog #hotelAddress').val(object['HotelAddress']);
    $('#adminAccomodationDialog #hotelContactNumber').val(object['HotelContactNumber']);
    $('#adminAccomodationDialog #bookingFromDate').val(object['BookingFromDate']);
    $('#adminAccomodationDialog #bookingToDate').val(object['BookingToDate']);
    $('#adminAccomodationDialog #roomDetails').val(object['RoomDetails']);
    $('#adminAccomodationDialog #checkinDetails').val(object['CheckinDetails']);
    $('#adminAccomodationDialog #checkoutDetails').val(object['CheckoutDetails']);
    $('#adminAccomodationDialog #additionalDetails').val(object['AdditionalDetails']);
    $('#adminAccomodationDialog #Accoupload').val('');
    $('#adminAccomodationDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 490,
        resizable: false,
        title: "Edit Accommodation Details"
    });
    $('#adminAccomodationDialog').dialog('open');
}

function DeleteAdminAccomodationDetail(accomodationId, travelId) {
    $('#DeleteAccomodationDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 300,
        height: 'auto',
        resizable: false,
        title: "Delete Accommodation Detail",
        dialogClass: "noclose",
        buttons:
		        {
		            "Ok": function () {
		                DisplayLoadingDialog(); //checked
		                $.ajax({
		                    url: "DeleteAdminAccomodationDetails/Travel",
		                    type: 'POST',
		                    data: { AccomodationID: accomodationId, TravelID: travelId },
		                    success: function (data) {
		                        $("#loading").dialog("close");
		                        $("#loading").dialog("destroy");
		                        $("#DeleteAccomodationDialog").dialog("close");
		                        if (data.status == true) {
		                            $("#DeleteAccomodationSuccess").dialog({
		                                modal: true,
		                                resizable: false,
		                                height: 140,
		                                width: 300,
		                                title: "Deleted",
		                                dialogClass: "noclose",
		                                buttons:
		                                {
		                                    "Ok": function () {
		                                        jQuery("#jqAccomodationAdminTabel").trigger("reloadGrid");
		                                        $(this).dialog('close');
		                                    }
		                                }
		                            });
		                        }
		                        else if (data.status == "Error") {
		                            $("#errorDialog").dialog({
		                                resizable: false,
		                                height: 'auto',
		                                width: 'auto',
		                                modal: true,
		                                title: 'Accommodation Details',
		                                dialogClass: "noclose",
		                                buttons: {
		                                    Ok: function () {
		                                        $(this).dialog("close");
		                                        jQuery("#jqAccomodationAdminTabel").trigger("reloadGrid");
		                                    }
		                                }
		                            }); //end dialog
		                        }
		                        else {
		                            $("#errorDialog").dialog({
		                                resizable: false,
		                                height: 'auto',
		                                width: 'auto',
		                                modal: true,
		                                title: 'Accommodation Details',
		                                dialogClass: "noclose",
		                                buttons: {
		                                    Ok: function () {
		                                        $(this).dialog("close");
		                                        jQuery("#jqAccomodationAdminTabel").trigger("reloadGrid");
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
    $('#DeleteAccomodationDialog').dialog('open');
}

// this function has to be removed -----------------------------------------------------prasad

function SaveNContinueAccoDetails(travelID, additionalInfo) {
    DisplayLoadingDialog(); //checked
    $.ajax({
        url: "SaveAdminAccoAdditionalInfo/Travel",
        type: "POST",
        data: { TravelID: travelID, AdditionalInformation: additionalInfo },
        success: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            if (data.status == true) {
                $("#SaveNContAccoDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Accommodation Details',
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            //                            window.location = ("GetConveyanceAdminFormDetails/Travel");
                            $('#newAccomodationAdmin_AdditionalInformation').val('');
                            var selected = $("#tabs").tabs("option", "selected");
                            $("#tabs").tabs("option", "selected", selected + 1);
                        }
                    }
                }); //end dialog
            }
            else if (data.status == "Error") {
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Accommodation Details',
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                }); //end dialog
            }
            else {
                $("#SaveNContinueErrorMessege").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Accommodation Details',
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                }); //end dialog
            }
        },
        error: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#errorDialog").dialog({
                resizable: false,
                height: 'auto',
                width: 'auto',
                modal: true,
                title: 'Accommodation Details',
                dialogClass: "noclose",
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            }); //end dialog
        }
    });
}