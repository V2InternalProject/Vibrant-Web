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

$(document).ready(function () {
    //    $('#fromdate').datepicker({ dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, yearRange: "-60:+10", showOn: "both", buttonImage: "../../Content/themes/base/images/calendar.gif", buttonImageOnly: true });
    //    $('#todate').datepicker({ dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, yearRange: "-60:+10", showOn: "both", buttonImage: "../../Content/themes/base/images/calendar.gif", buttonImageOnly: true });
});

function DeleteConveyanceDetail(visattravelId, Id) {
    $('#DeleteConfirmationDialog').dialog(
			{
			    autoOpen: false,
			    modal: true,
			    height: 'auto',
			    width: 300,
			    resizable: false,
			    title: "Delete Visa Travel Detail",
			    dialogClass: "noclose",
			    buttons:
					{
					    Ok: function () {
					        $.ajax({
					            url: 'DeleteConvaynaceDetails/Travel',
					            data: { ConvaytravelId: visattravelId, travelId: Id },
					            success: function (data) {
					                if (data.status == true) {
					                    $("#DeleteConfirmationDialog").dialog("close");
					                    $("#DeleteConfirmation").dialog({
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
					                                jQuery("#ConveyanceDetailsTable").trigger("reloadGrid");
					                            }
					                        }
					                    });
					                }
					                else if (data.status == "Error") {
					                    $("#DeleteConfirmationDialog").dialog("close");
					                    $("#errorDialog").dialog({
					                        resizable: false,
					                        height: 'auto',
					                        width: 'auto',
					                        modal: true,
					                        title: 'Visa Travel Details',
					                        buttons: {
					                            Ok: function () {
					                                $(this).dialog("close");
					                                jQuery("#ConveyanceDetailsTable").trigger("reloadGrid");
					                            }
					                        }
					                    }); //end dialog
					                }
					                else {
					                    $("#DeleteConfirmationDialog").dialog("close");
					                    $("#errorDialog").dialog({
					                        resizable: false,
					                        height: 'auto',
					                        width: 'auto',
					                        modal: true,
					                        title: 'Declaration Details',
					                        buttons: {
					                            Ok: function () {
					                                $(this).dialog("close");
					                                jQuery("#ConveyanceDetailsTable").trigger("reloadGrid");
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
    $('#DeleteConfirmationDialog').dialog('open');
}

//Edit dependent details
function EditConvaeyDetails(Object) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $('#addConveyanceDetailsDialog #TravelID').val(Object['TravelID']);
    $("#addConveyanceDetailsDialog #ddlConvayanaceType option[value = " + Object['ConveyanceType'] + "] ").attr('selected', 'selected');
    $("#addConveyanceDetailsDialog #ddlCity  option[value = " + Object['City'] + "] ").attr('selected', 'selected');

    $('#addConveyanceDetailsDialog #hdnCoveyanceType').val(Object['ConveyanceType']);
    $('#addConveyanceDetailsDialog #hdnCity').val(Object['City']);
    $('#addConveyanceDetailsDialog #hdnCity').val(Object['CityName']);
    $('#addConveyanceDetailsDialog #ddlCity').val(Object['CityName']);
    $('#addConveyanceDetailsDialog #LocalConveyanceID').val(Object['LocalConveyanceID']);
    $('#addConveyanceDetailsDialog #TravelDetails').val(Object['TravelDetails']);
    $('#addConveyanceDetailsDialog #txtTraveldetails').val(Object['TravelDetails']);
    $('#addConveyanceDetailsDialog #hdnTraveldetails').val(Object['TravelDetails']);
    $('#addConveyanceDetailsDialog #conveyancefromdate').val(Object['FromDate']);
    $('#addConveyanceDetailsDialog #fromDate').val(Object['FromDate']);
    $('#addConveyanceDetailsDialog #toDate').val(Object['ToDate']);
    $('#addConveyanceDetailsDialog #todate').val(Object['ToDate']);
    $('#addConveyanceDetailsDialog #InsuranceDetails').val(Object['InsuranceDetails']);
    $('#addConveyanceDetailsDialog #txtinsurance').val(Object['InsuranceDetails']);
    $('#addConveyanceDetailsDialog #hdninsurencedeatails').val(Object['InsuranceDetails']);

    $('#addConveyanceDetailsDialog #AirportName').val(Object['AirportName']);
    $('#addConveyanceDetailsDialog #hdnAirportName').val(Object['AirportName']);

    $('#addConveyanceDetailsDialog #HotelName').val(Object['HotelName']);
    $('#addConveyanceDetailsDialog #hdnHotelName').val(Object['HotelName']);

    var ddlVal = $('#ddlConvayanaceType option:selected').text();
    if (ddlVal == 'Shuttle') {
        $('#ReservationNum').show();
        $('#addConveyanceDetailsDialog #ReservationNumber').val(Object['ReservationNumber']);
        $('#addConveyanceDetailsDialog #hdnReservationNumber').val(Object['ReservationNumber']);
    }
    else {
        $('#ReservationNum').hide();
        $('#ReservationNumber').val('');
    }
    $('#addConveyanceDetailsDialog #FromAddress').val(Object['FromAddress']);
    $('#addConveyanceDetailsDialog #hdnFromAddress').val(Object['FromAddress']);

    $('#addConveyanceDetailsDialog #ToAddress').val(Object['ToAddress']);
    $('#addConveyanceDetailsDialog #hdnToAddress').val(Object['ToAddress']);

    if (Object['AirporttoHotel'] == 1) {
        $('#txtAirporttoHotel').attr('checked', true);
    }
    if (Object['AirporttoHotel'] == 2) {
        $('#txtHoteltoAirport').attr('checked', true);
    }

    $('#addConveyanceDetailsDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Edit Travel Visa Details"
    }
        );
    $('#addConveyanceDetailsDialog').dialog('open');
}

//Reset button on dependent
var ResetConveynacedetails = function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");

    if ($('#hdnCoveyanceType').val() == "") {
        $("#ddlConvayanaceType option[value = " + $('#hdnCoveyanceType').val() + "] ").attr('selected', 'selected');
    }
    else {
        $("#ddlConvayanaceType option:contains(" + $('#hdnCoveyanceType').val() + ')').attr('selected', 'selected');
    }

    $("#ddlConvayanaceType option[value = " + $('#hdnCoveyanceType').val() + "] ").attr('selected', 'selected');
    $("#ConveyanceType option[value = " + $('#hdnCoveyanceType').val() + "] ").attr('selected', 'selected');

    //    if ($('#hdnCity').val() == "") {
    //        $("#ddlCity option[value = " + $('#hdnCity').val() + "] ").attr('selected', 'selected');
    //    }
    //    else {
    //        $("#ddlCity option:contains(" + $('#hdnCity').val() + ')').attr('selected', 'selected');
    //    }
    //    $("#City option[value = " + $('#hdnCity').val() + "] ").attr('selected', 'selected');
    //    $("#ddlCity option[value = " + $('#hdnCity').val() + "] ").attr('selected', 'selected');
    $("#ddlCity").val($('#hdnCity').val());
    $("#conveyancefromdate").val($('#fromDate').val());
    $("#todate").val($('#toDate').val());
    $("#TravelDetails").val($('#hdnTraveldetails').val());
    $("#InsuranceDetails").val($('#hdninsurencedeatails').val());

    $("#FromAddress").val($('#hdnFromAddress').val());
    $("#ToAddress").val($('#hdnToAddress').val());
    $("#ReservationNumber").val($('#hdnReservationNumber').val());

    $("#AirportName").val($('#hdnAirportName').val());
    $("#HotelName").val($('#hdnHotelName').val());
}

function SaveConveynanceDetails() {
    var ddlVal = $('#ddlConvayanaceType option:selected').text();

    if (ddlVal == 'Shuttle') {
        var resernum = $("#ReservationNumber").val().trim();
        if (resernum == "") {
            $("#ReservationNumError").dialog({
                resizable: false,
                height: 140,
                width: 300,
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }
    }

    if ($('#addConveynanceDetailsadmin').valid()) {
        DisplayLoadingDialog(); //checked

        $.ajax({
            url: "SaveConvaynanceDetailsInfo/Travel",
            type: 'POST',
            data: $('#addConveynanceDetailsadmin').serialize(),
            success: function (results) {
                if (results.status == true) {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    $('#addConveyanceDetailsDialog').dialog("close");
                    jQuery("#ConveyanceDetailsTable").trigger("reloadGrid");
                    $("#AddTravelConveyanceDetailsSuccessMessege").dialog({
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
                else if (results.status == "Error") {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    $('#addTravelVisaDetailsDialog').dialog("close");
                    $("#errorDialog").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: 'Travel Visa Details',
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                                jQuery("#ConveyanceDetailsTable").trigger("reloadGrid");
                            }
                        }
                    }); //end dialog
                }
                else {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    $('#addTravelVisaDetailsDialog').dialog("close");
                    $("#AddTravelVisaDetailsErrorMessege").dialog({
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                                jQuery("#ConveyanceDetailsTable").trigger("reloadGrid");
                            }
                        }
                    });
                }
            }
        });
        return true;
    }
    else
        return false;
}