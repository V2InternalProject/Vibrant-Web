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
    $('#fromdate').datepicker({ dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, yearRange: "-60:+10", showOn: "both", buttonImage: "../../Content/themes/base/images/calendar.gif", buttonImageOnly: true });
    $('#todateVisaDetails').datepicker({ dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, yearRange: "-60:+10", showOn: "both", buttonImage: "../../Content/themes/base/images/calendar.gif", buttonImageOnly: true });
});

function DeleteVisatravelDetail(visattravelId, Id) {
    $('#DeleteConfirmationDialog').dialog(
			{
			    autoOpen: false,
			    modal: true,
			    width: 300,
			    height: 125,
			    resizable: false,
			    title: "Delete Visa Travel Detail",
			    buttons:
					{
					    Ok: function () {
					        $.ajax({
					            url: 'DeleteTravelVisaDetails/Travel',
					            data: { TravelId: visattravelId, IDs: Id },
					            success: function (data) {
					                if (data.status == true) {
					                    $("#DeleteConfirmationDialog").dialog("close");
					                    $("#DeleteConfirmation").dialog({
					                        modal: true,
					                        resizable: false,
					                        height: 140,
					                        width: 300,
					                        title: "Deleted",
					                        buttons:
					                        {
					                            "Ok": function () {
					                                $(this).dialog('close');
					                                jQuery("#VisaTravelDetailsTable").trigger("reloadGrid");
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
					                                jQuery("#VisaTravelDetailsTable").trigger("reloadGrid");
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
					                                jQuery("#VisaTravelDetailsTable").trigger("reloadGrid");
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

function EditTravelVisaDetails(Object, visattravelId) {
    $(".field-validation-error").empty();
    $('#addTravelVisaDetailsDialog #todateVisaDetails').datepicker('disable');
    $('input').removeClass("input-validation-error");
    $('#addTravelVisaDetailsDialog #VisaTravelID').val(Object['VisaTravelID']);
    $('#addTravelVisaDetailsDialog #VisaTravelIDnew').val(Object['VisaTravelID']);
    $('#addTravelVisaDetailsDialog #VisaTravelIDnew').val(visattravelId);

    $("#addTravelVisaDetailsDialog #VisaDetails_CountryID option[value = " + Object['CountryID'] + "] ").attr('selected', 'selected');
    $("#addTravelVisaDetailsDialog #Visa_VisaTypeID  option[value = " + Object['VisaTypeID'] + "] ").attr('selected', 'selected');
    $('#addTravelVisaDetailsDialog #visaCountry').val(Object['CountryID']);
    $('#addTravelVisaDetailsDialog #VisaTypeName').val(Object['VisaTypeID']);
    $('#addTravelVisaDetailsDialog #fromdate').val(Object['FromDate']);
    $('#addTravelVisaDetailsDialog #fromDate').val(Object['FromDate']);
    $('#addTravelVisaDetailsDialog #todateVisaDetails').val(Object['ToDate']);
    $('#addTravelVisaDetailsDialog #hdntoDate').val(Object['ToDate']);
    $('#addTravelVisaDetailsDialog #Decription').val(Object['Decription']);
    $('#addTravelVisaDetailsDialog #visaDecription').val(Object['Decription']);

    $('#addTravelVisaDetailsDialog #AdditionalInfo').val(Object['AdditionalInfo']);
    $('#addTravelVisaDetailsDialog #additionalInfo').val(Object['AdditionalInfo']);
    $('#addTravelVisaDetailsDialog #VisaTravelID').removeAttr("disabled");
    $('#addTravelVisaDetailsDialog #VisaTravelIDnew').removeAttr("disabled");
    $('#addTravelVisaDetailsDialog #VisaTravelID').val(Object['VisaTravelID']);
    $('#addTravelVisaDetailsDialog #VisaTravelIDnew').val(Object['VisaTravelID']);
    $('#addTravelVisaDetailsDialog #VisaDetails_CountryID').removeAttr("disabled");
    $('#addTravelVisaDetailsDialog #Visa_VisaTypeID').removeAttr("disabled");
    $('#addTravelVisaDetailsDialog #Decription').removeAttr("disabled");
    if (Object['VisaAddedStatus'] == '') {
        $('#addTravelVisaDetailsDialog #VisaDetails_CountryID').attr("disabled", true);
        $('#addTravelVisaDetailsDialog #Visa_VisaTypeID').attr("disabled", true);
        $('#addTravelVisaDetailsDialog #todateVisaDetails').attr("disabled", true);
        $('#addTravelVisaDetailsDialog #adminVisa').removeAttr("disabled");
        $('#addTravelVisaDetailsDialog #Decription').removeAttr("disabled");
        $('#addTravelVisaDetailsDialog #isAdminRecord').val('');
    }
    if ($("#StageID").val() == "4") {
        $('#addTravelVisaDetailsDialog #VisaDetails_CountryID').attr("disabled", true);
        $('#addTravelVisaDetailsDialog #Visa_VisaTypeID').attr("disabled", true);
        $('#addTravelVisaDetailsDialog #todateVisaDetails').attr("disabled", true);
        $('#addTravelVisaDetailsDialog #adminVisa').attr("disabled", true);
        $('#addTravelVisaDetailsDialog #Decription').removeAttr("disabled");
    }
    $('#addTravelVisaDetailsDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Edit Travel Visa Details",
        open: function () {
            if (Object['VisaAddedStatus'] == 1) {
                $('#addTravelVisaDetailsDialog #todateVisaDetails').datepicker('enable');
                $('#addTravelVisaDetailsDialog #adminVisa').removeAttr("disabled");
                $('#addTravelVisaDetailsDialog #isAdminRecord').val('1');
            }
            if ($('#addTravelVisaDetailsDialog #isAdminRecord').val() == '1')
                $('#addTravelVisaDetailsDialog #travelVisaAsterisk').show();
            else
                $('#addTravelVisaDetailsDialog #travelVisaAsterisk').hide();
            if ($("#StageID").val() == "4") {
                $('#addTravelVisaDetailsDialog #VisaDetails_CountryID').attr("disabled", true);
                $('#addTravelVisaDetailsDialog #Visa_VisaTypeID').attr("disabled", true);
                $('#addTravelVisaDetailsDialog #todateVisaDetails').attr("disabled", true);
                $('#addTravelVisaDetailsDialog #adminVisa').attr("disabled", true);
                $('#addTravelVisaDetailsDialog #Decription').removeAttr("disabled");
                $('#addTravelVisaDetailsDialog #travelVisaAsterisk').hide();
            }
        }
    }
        );
    $('#addTravelVisaDetailsDialog').dialog('open');
}

//Reset button on dependent
var emptyDialogTravelVisaDetails = function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");

    if ($('#visaCountry').val() == "") {
        $("#CountryID option[value = " + $('#visaCountry').val() + "] ").attr('selected', 'selected');
    }
    else {
        $("#CountryID option:contains(" + $('#visaCountry').val() + ')').attr('selected', 'selected');
    }
    $("#VisaDetails_CountryID option[value = " + $('#visaCountry').val() + "] ").attr('selected', 'selected');

    if ($('#VisaTypeName').val() == "") {
        $("#VisaTypeID option[value = " + $('#VisaTypeName').val() + "] ").attr('selected', 'selected');
    }
    else {
        $("#VisaTypeID option:contains(" + $('#VisaTypeName').val() + ')').attr('selected', 'selected');
    }
    $("#Visa_VisaTypeID option[value = " + $('#VisaTypeName').val() + "] ").attr('selected', 'selected');

    $("#fromdate").val($('#fromDate').val());
    $("#todateVisaDetails").val($('#hdntoDate').val());
    $("#Decription").val($('#visaDecription').val());

    $("#AdditionalInfo").val($('#additionalInfo').val());
    $("#adminVisa").val("");
    $("#FalseVisaFileUploadTxt").val("No files selected");
}

$('#saveTravelVisadetails').click(function () {
    //    if (($("#isAdminRecord").val() == "1" && ($('#adminVisa').val() != "" && $('#adminVisa').val() != "null") && $("#StageID").val() == "3") || $("#isAdminRecord").val() == "" || $("#StageID").val() == "4") {
    if ($("#adminVisa").val() != "") {
        //DisplayLoadingDialog(); //checked
        $("#VisaUploadErrorAdmin").text("");
        $("#addTravelVisaDetails").ajaxForm({
            success: function (results) {
                $("#addTravelVisaDetailsDialog").dialog('close');
                //                $("#loading").dialog("close");
                //                $("#loading").dialog("destroy");
                var st = $.parseJSON(results);
                if (st.status == true) {
                    $("#adminVisa").replaceWith($("#adminVisa").clone(true));
                    if (st.VisaId == 0)
                        $("#VisaTravelDetailsTable").find('label[id="undefined_UploadedFileName"]').text(st.FileName).show();
                    else
                        $("#VisaTravelDetailsTable").find('label[id="' + st.VisaId + "_UploadedFileName" + '"]').text(st.FileName).show();
                    $('#AdminVisaSuccess').dialog({
                        modal: true,
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        dialogClass: "noclose",
                        buttons: {
                            "OK": function () {
                                $(this).dialog("close");
                                //jQuery("#VisaTravelDetailsTable").trigger("reloadGrid");
                                $('#adminVisa').val('');
                            }
                        }
                    });
                }
                else if (st.status == "Error") {
                    DisplayErrorDialog("Add Visa Details");
                }
                else {
                    $('#errorUploadAdminVisaDialog').dialog({
                        modal: true,
                        resizable: false,
                        height: 'auto',
                        width: 300,
                        title: "Error",
                        dialogClass: "noclose",
                        open: function () {
                            $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                            $(this).parent().css('z-index', '27');
                        },
                        buttons: {
                            "OK": function () { $(this).dialog('close'); }
                        }
                    });
                }
            },
            error: function () {
                //                $("#loading").dialog("close");
                //                $("#loading").dialog("destroy");
                $('#errorUploadAdminVisaDialog').dialog({
                    modal: true,
                    resizable: false,
                    height: 'auto',
                    width: 300,
                    title: "Error",
                    dialogClass: "noclose",
                    buttons: {
                        "OK": function () { $(this).dialog('close'); }
                    }
                });
            }
        });
    }
    else {
        $("#VisaUploadErrorAdmin").text(" Please select File to upload");
        return false;
    }
});

function LinkTravelVisaClick(event, EmployeeID) {
    DisplayLoadingDialog(); //checked
    var VisaTravelID = event.id;
    $.ajax({
        url: "ShowTravelVisaDetails/EmployeeDetails",
        data: { VisaTravelID: VisaTravelID },
        type: 'GET',
        success: function (result) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#ShowTravelVisaDetailsDiv").empty().append(result);
            $("#ShowTravelVisaDetailsDiv").dialog({
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                modal: true,
                width: '300',
                title: "Visa History",
                buttons: {
                    "Ok": function () {
                        $("#loading").dialog("close");
                        $("#loading").dialog("destroy");
                        $('#ShowTravelVisaDetailsDiv').dialog('close');
                        $('#ShowTravelVisaDetailsDiv').dialog('destroy');
                        $("#VisaTravelDetailsTable").trigger("reloadGrid");
                    }
                },
                close: function (event, ui) {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    $("#ShowTravelVisaDetailsDiv").empty();
                    $("#ShowTravelVisaDetailsDiv").dialog("destroy");
                    $("#VisaTravelDetailsTable").trigger("reloadGrid");
                }
            });
        }
    });
}

function UploadVisaFunction() {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#addTravelVisaDetailsDialog #FalseVisaFileUploadTxt").val("No files selected");
    $("#addTravelVisaDetailsDialog #adminVisa").replaceWith($("#addTravelVisaDetailsDialog #adminVisa").val("").clone(true));
    $('#addTravelVisaDetailsDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Travel Visa Details"
    });
    $('#addTravelVisaDetailsDialog').dialog('open');
}