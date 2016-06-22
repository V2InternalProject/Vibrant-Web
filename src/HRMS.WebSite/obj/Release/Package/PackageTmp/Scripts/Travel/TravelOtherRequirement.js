$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

function DeleteOthertravelDetail(visattravelId, Id) {
    $('#DeleteConfirmationDialog').dialog(
			{
			    autoOpen: false,
			    modal: true,
			    width: 300,
			    height: 'auto',
			    resizable: false,
			    title: "Delete Visa Travel Detail",
			    dialogClass: "noclose",
			    buttons:
					{
					    Ok: function () {
					        $.ajax({
					            url: 'DeleteTravelOtherRequiementDetails/Travel',
					            data: { TravelId: visattravelId, IDs: Id },
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
					                                jQuery("#VisaTravelOtherDetailsTable").trigger("reloadGrid");
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
					                                jQuery("#VisaTravelOtherDetailsTable").trigger("reloadGrid");
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
					                                jQuery("#VisaTravelOtherDetailsTable").trigger("reloadGrid");
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
function EditTravelOtherDetails(Object) {
    $('#addTravelOtherDetailsDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 540,
        resizable: false,
        title: "Edit Miscellaneous Details"
    });
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $('#addTravelOtherDetailsDialog #RequirementID').val(Object['RequirementID']);
    $("#addTravelOtherDetailsDialog #requrementTypeID_id").next().find('.selectBox-label').text(Object['Description']);
    $("#addTravelOtherDetailsDialog #requrementTypeID_id option[value = " + Object['RequrementTypeID'] + "] ").attr('selected', 'selected');
    $("#addTravelOtherDetailsDialog #acceptance_Id  option[value = " + Object['AcceptanceID'] + "] ").attr('selected', 'selected');
    $("#addTravelOtherDetailsDialog #acceptance_Id").next().find('.selectBox-label').text(Object['ReceivedByEmployee']);
    $("#addTravelOtherDetailsDialog #CurrencyId option[value = " + Object['CurrencyID'] + "] ").attr('selected', 'selected');
    $("#addTravelOtherDetailsDialog #CurrencyId").next().find('.selectBox-label').text(Object['CurrnyName']);
    $('#addTravelOtherDetailsDialog #hdnCurcyid').val(Object['CurrencyID']);
    $('#addTravelOtherDetailsDialog #descriptionid').val(Object['RequrementTypeID']);
    $('#addTravelOtherDetailsDialog #acceptance').val(Object['AcceptanceID']);

    $('#addTravelOtherDetailsDialog #empPassport').val('');
    $('#addTravelOtherDetailsDialog #DetailID').val(Object['Miscdetails']);
    $('#addTravelOtherDetailsDialog #details').val(Object['Miscdetails']);
    $("#addTravelOtherDetailsDialog #FileEmpPassportField").val("No files selected");
    $('#addTravelOtherDetailsDialog #empPassport').replaceWith($('#addTravelOtherDetailsDialog #empPassport').val("").clone(true));
    var ddlVal = $('#requrementTypeID_id option:selected').text();
    if (ddlVal == 'Advances') {
        $(".DdlAdvances").show();
        if (Object['Advacesamount'] != "") {
            $('#cash').prop('checked', true);
            $('#addTravelOtherDetailsDialog #Advacesamount').val(Object['Advacesamount']);
            $('#addTravelOtherDetailsDialog #hdnamount').val(Object['Advacesamount']);
        }
        else {
            $('#cash').prop('checked', false);
            $('#addTravelOtherDetailsDialog #Advacesamount').val('');
            $('.HideCash').hide();
        }
        if (Object['AmountOnCard'] != "" || Object['CardDetails'] != "") {
            $('#card').prop('checked', true);
            $('#addTravelOtherDetailsDialog #AmountOnCard').val(Object['AmountOnCard']);
            $('#addTravelOtherDetailsDialog #hdnAmountOnCard').val(Object['AmountOnCard']);
            $('#addTravelOtherDetailsDialog #CardDetails').val(Object['CardDetails']);
            $('#addTravelOtherDetailsDialog #hdnCardDetails').val(Object['CardDetails']);
        }
        else {
            $('#card').prop('checked', false);
            $('#addTravelOtherDetailsDialog #AmountOnCard').val('');
            $('#addTravelOtherDetailsDialog #CardDetails').val('');
            $('.HideCard').hide();
        }
    }
    else {
        $(".DdlAdvances").hide();
    }

    if (ddlVal == 'Medical Insurance') {
        $(".DdlInsurance").show();
        $('#addTravelOtherDetailsDialog #hdnInsuranceToDate').val(Object['InsuranceToDate']);
        $('#addTravelOtherDetailsDialog #InsuranceToDate').val(Object['InsuranceToDate']);
        $('#addTravelOtherDetailsDialog #InsuranceFromDate').val(Object['InsuranceFromDate']);
        $('#addTravelOtherDetailsDialog #hdnInsuranceFromDate').val(Object['InsuranceFromDate']);
    }
    else {
        $(".DdlInsurance").hide();
    }

    $('#addTravelOtherDetailsDialog').dialog('open');
}

//Reset button on dependent
var emptyDialogTravelOtherDetails = function () {
    $("#empPassport").replaceWith($("#empPassport").clone(true));

    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");

    if ($('#descriptionid').val() == "") {
        $("#RequrementTypeID option[value = " + $('#descriptionid').val() + "] ").attr('selected', 'selected');
    }
    else {
        $("#RequrementTypeID option:contains(" + $('#descriptionid').val() + ')').attr('selected', 'selected');
    }
    $("#requrementTypeID_id option[value = " + $('#descriptionid').val() + "] ").attr('selected', 'selected');

    if ($('#acceptance').val() == "") {
        $("#AcceptanceID option[value = " + $('#acceptance').val() + "] ").attr('selected', 'selected');
    }
    else {
        $("#AcceptanceID option:contains(" + $('#acceptance').val() + ')').attr('selected', 'selected');
    }
    $("#acceptance_Id option[value = " + $('#acceptance').val() + "] ").attr('selected', 'selected');

    if ($('#hdnCurcyid').val() == "") {
        $("#CurrencyID option[value = " + $('#hdnCurcyid').val() + "] ").attr('selected', 'selected');
    }
    else {
        $("#CurrencyID option:contains(" + $('#hdnCurcyid').val() + ')').attr('selected', 'selected');
    }
    $("#CurrencyId option[value = " + $('#hdnCurcyid').val() + "] ").attr('selected', 'selected');

    var ddlVal = $('#requrementTypeID_id option:selected').text();
    if (ddlVal == 'Advances') {
        $(".DdlAdvances").show();
        if ($("#cash").is(':checked') == true) {
            $("#Advacesamount").val($('#hdnamount').val());
        }
        else {
            $("#Advacesamount").val('');
            $('.HideCash').hide();
            $('.resetshowcash').show();
        }
        if ($("#card").is(':checked') == true) {
            $('#addTravelOtherDetailsDialog #AmountOnCard').val($('#hdnAmountOnCard').val());
            $('#addTravelOtherDetailsDialog #CardDetails').val($('#hdnCardDetails').val());
        }
        else {
            $('#addTravelOtherDetailsDialog #AmountOnCard').val('');
            $('#addTravelOtherDetailsDialog #CardDetails').val('');
            $('.HideCard').hide();
            $('.resetshowcard').show();
        }
    }
    else {
        $(".DdlAdvances").hide();
    }

    if (ddlVal == 'Medical Insurance') {
        $(".DdlInsurance").show();
        $('#addTravelOtherDetailsDialog #InsuranceFromDate').val($('#hdnInsuranceFromDate').val());
        $('#addTravelOtherDetailsDialog #InsuranceToDate').val($('#hdnInsuranceToDate').val());
    }
    else {
        $(".DdlInsurance").hide();
    }

    $("#DetailID").val($('#details').val());
}

function ajaxCallFunction(stageID) {
    if (stageID == 4)
        $("#addTravelOtherDetails").find("input,select").removeAttr("disabled");

    $('#addTravelOtherDetails').ajaxForm({
        async: false,
        datatype: "json",
        success: function (results) {
            if (stageID == 4) {
                $("#addTravelOtherDetails").find("input,select").attr("disabled", "disabled");
                //$('#acceptance_Id').removeAttr("disabled");
                $('#acceptance_Id').selectBox("enable");
                $(".resetClass").removeAttr("disabled");
                $("#saveTravelOtherdetails").removeAttr("disabled");
            }
            var results = $.parseJSON(results);
            if (results.isFormValid == false) {
            }
            else if (results.status == true) {
                $("#empPassport").replaceWith($("#empPassport").clone(true));
                $('#addTravelOtherDetailsDialog').dialog("close");
                jQuery("#VisaTravelOtherDetailsTable").trigger("reloadGrid");
                $("#AddTravelOtherDetailsSuccessMessege").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 300,
                    modal: true,
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
            else if (results.status == "Error") {
                $('#addTravelOtherDetailsDialog').dialog("close");
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Travel Visa Details',
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            jQuery("#VisaTravelOtherDetailsTable").trigger("reloadGrid");
                        }
                    }
                }); //end dialog
            }
            else {
                $('#addTravelOtherDetailsDialog').dialog("close");
                $("#AddTravelOtherDetailsErrorMessege").dialog({
                    resizable: false,
                    height: 140,
                    width: 300,
                    modal: true,
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            jQuery("#VisaTravelOtherDetailsTable").trigger("reloadGrid");
                        }
                    }
                });
            }
        }
    });
    //return true;
}

function saveTravelOtherdetailsFunction(stageID) {
    var ddlVal = $('#requrementTypeID_id option:selected').text();
    if (new Date($('#addTravelOtherDetailsDialog #InsuranceFromDate').val()) > new Date($('#addTravelOtherDetailsDialog #InsuranceToDate').val())) {
        $("#ErrorInsuranceDate").dialog({
            resizable: false,
            height: 'auto',
            width: 300,
            modal: true,
            dialogClass: "noclose",
            open: function () {
                $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                $(this).parent().css('z-index', '27');
            },
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                }
            }
        });
        $("#addTravelOtherDetails #isFormValid").val(false);

        ajaxCallFunction(stageID);
        return false;
    }
    else {
        $("#addTravelOtherDetails #isFormValid").val(true);
    }

    if (ddlVal == 'Medical Insurance') {
        if (($('#addTravelOtherDetailsDialog #InsuranceFromDate').val() == "") || ($('#addTravelOtherDetailsDialog #InsuranceToDate').val() == "")) {
            $("#ErrorInsuranceDateRequired").dialog({
                resizable: false,
                height: 'auto',
                width: 300,
                modal: true,
                dialogClass: "noclose",
                open: function () {
                    $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                    $(this).parent().css('z-index', '27');
                },
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#addTravelOtherDetails #isFormValid").val(false);

            ajaxCallFunction(stageID);
            return false;
        }
        else {
            $("#addTravelOtherDetails #isFormValid").val(true);
        }
    }

    if (ddlVal == 'Advances') {
        if ($('#CurrencyId').val() == "") {
            $("#ErrorCurrency").dialog({
                resizable: false,
                height: 140,
                width: 300,
                modal: true,
                dialogClass: "noclose",
                open: function () {
                    $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                    $(this).parent().css('z-index', '27');
                },
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#addTravelOtherDetails #isFormValid").val(false);
            ajaxCallFunction(stageID);
            return false;
        }
        else {
            $("#addTravelOtherDetails #isFormValid").val(true);
        }
    }

    if (ddlVal == 'Advances') {
        if (($("#cash").is(':checked') != true) && ($("#card").is(':checked') != true)) {
            $("#ErrorAdvances").dialog({
                resizable: false,
                height: 140,
                width: 300,
                modal: true,
                dialogClass: "noclose",
                open: function () {
                    $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                    $(this).parent().css('z-index', '27');
                },
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#addTravelOtherDetails #isFormValid").val(false);
            ajaxCallFunction(stageID);
            return false;
        }
        else {
            $("#addTravelOtherDetails #isFormValid").val(true);
        }
    }

    if ($("#cash").is(':checked') == true) {
        if ($('#Advacesamount').val().trim() == "") {
            $("#ErrorAmountInCash").dialog({
                resizable: false,
                height: 140,
                width: 300,
                modal: true,
                dialogClass: "noclose",
                open: function () {
                    $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                    $(this).parent().css('z-index', '27');
                },
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#addTravelOtherDetails #isFormValid").val(false);
            ajaxCallFunction(stageID);
            return false;
        }
        else {
            $("#addTravelOtherDetails #isFormValid").val(true);
        }
    }
    if ($("#card").is(':checked') == true) {
        if ($('#CardDetails').val().trim() == "" || $('#AmountOnCard').val().trim() == "") {
            $("#ErrorAmountCardnCardDetails").dialog({
                resizable: false,
                height: 140,
                width: 300,
                modal: true,
                dialogClass: "noclose",
                open: function () {
                    $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                    $(this).parent().css('z-index', '27');
                },
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#addTravelOtherDetails #isFormValid").val(false);
            ajaxCallFunction(stageID);
            return false;
        }
        else {
            $("#addTravelOtherDetails #isFormValid").val(true);
        }
    }
    if ($("#addTravelOtherDetails #isFormValid").val() == 'true') {
        ajaxCallFunction(stageID);
        return false;
    }
}