$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

$(document).ready(function () {
    //    $('#JourneyDate').datepicker({ dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, yearRange: "-60:+10", showOn: "both", buttonImage: "../../Content/themes/base/images/calendar.gif", buttonImageOnly: true });
});

//save

function SaveJourneyDetails() {
    //    if ($('#addTravelsJourneyDetails').valid()) {
    //        $.ajax({
    $('#JourneyFeedback').val($('#journeyFeedback').val());
    $('#addTravelsJourneyDetails').ajaxForm({
        success: function (results) {
            var results = $.parseJSON(results);
            if (results.status == true) {
                $("#empTicket").replaceWith($("#empTicket").clone(true));
                $('#addJourneyDialog').dialog("close");
                //jQuery("#JourneyTable").trigger("reloadGrid");
                if (results.JourneyId == 0)
                    $("#JourneyTable").find('label[id="undefined_UploadedFileName"]').text(results.FileName).show();
                else
                    $("#JourneyTable").find('label[id="' + results.JourneyId + "_UploadedFileName" + '"]').text(results.FileName).show();
                $("#JourneyUploadSuccessMessege").dialog({
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
                $('#addJourneyDialog').dialog("close");
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Journey Details',
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                }); //end dialog
            }
            else {
                $('#addJourneyDialog').dialog("close");
                $("#JourneyUploadErrorMessege").dialog({
                    resizable: false,
                    height: 140,
                    width: 300,
                    modal: true,
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            //jQuery("#journeyTable").trigger("reloadGrid");
                        }
                    }
                });
            }
        }
    });
    return true;
}

function resetJourneyDetails() {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#FalseJourneyFileUploadBtn").val("No files selected");
    $('#empTicket').replaceWith($('#empTicket').val("").clone(true));
}

$('#empTicket').change(function () {
    var file = $('input[type="file"]').val();
    var exts = ['exe'];
    // first check if file field has any value
    if (file) {
        $("#JourneyUploadError").text("");
        // split file name at dot
        var get_ext = file.split('.');
        // reverse name to check extension
        get_ext = get_ext.reverse();
        // check file type is valid as given in 'exts' array
        if ($.inArray(get_ext[0].toLowerCase(), exts) == 0) {
            $("#EmpJourneyUploadError").dialog({
                title: 'Error',
                resizable: false,
                height: 'auto',
                width: 300,
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");

                        if ($.browser.msie) {
                            $('#empTicket').replaceWith($('#empTicket').clone(true));
                        } else {
                            $('#empTicket').val('');
                        }
                    }
                },
                close: function () {
                    if ($.browser.msie) {
                        $('#empTicket').replaceWith($('#empTicket').clone(true));
                    } else {
                        $('#empTicket').val('');
                    }
                }
            });
        }
    }
});

//Edit client details
function EditJourneyDetails(Object) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    var selecttext = Object['JourneyMode'];
    if (selecttext == 'Bus' || selecttext == 'Cab' || selecttext == 'Shuttle' || selecttext == 'Other') {
        $('#addTravelsJourneyDetails #empTicket').attr('disabled', 'disabled');
    }
    else {
        $('#addTravelsJourneyDetails #empTicket').removeAttr("disabled");
    }

    if ($('#StageID').val() == 0) {
        $('#addTravelsJourneyDetails #empTicket').attr('disabled', 'disabled');
    }

    $('#addTravelsJourneyDetails #JourneyDetail_FromPlace').val(Object['FromPlace']);
    $('#addTravelsJourneyDetails #JourneyDetail_ToPlace').val(Object['ToPlace']);
    $('#addTravelsJourneyDetails #JourneyDate').val(Object['JourneyDate']);
    $('#addTravelsJourneyDetails #JourneyDetail_AdditionalInformation').val(Object['AdditionalInformation']);
    $('#addTravelsJourneyDetails #JourneyDetail_JourneyModeDetails').val(Object['JourneyModeDetails']);
    $('#addTravelsJourneyDetails #JourneyDetail_JourneyFeedback').val(Object['JourneyFeedback']);
    $('#addTravelsJourneyDetails #journeyFeedback').val(Object['JourneyFeedback']);
    $('#addTravelsJourneyDetails #JourneyId').val(Object['JourneyID']);
    $('#addTravelsJourneyDetails #empTicket').val('');
    $("#addTravelsJourneyDetails #JourneyDetail_JourneyMode option[value = " + Object['JourneyModeID'] + "] ").attr('selected', 'selected');
    $('#addJourneyDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Edit Journey Details"
    });
    $('#addJourneyDialog').dialog('open');
}

function DeleteJourneyDetail(journeyID) {
    $('#deleteJourneyDialogConfirmation').dialog(
			{
			    autoOpen: false,
			    modal: true,
			    width: 300,
			    height: 'auto',
			    resizable: false,
			    title: "Delete Journey Detail",
			    dialogClass: "noclose",
			    buttons:
					{
					    Ok: function () {
					        $.ajax({
					            url: "DeleteJourneyDetails/Travel",
					            data: { journeyId: journeyID },
					            success: function (data) {
					                if (data.status == true) {
					                    $("#deleteJourneyDialogConfirmation").dialog("close");
					                    $("#deleteJourneyRecord").dialog({
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
					                                jQuery("#JourneyTable").trigger("reloadGrid");
					                            }
					                        }
					                    });
					                }
					                else if (data.status == "Error") {
					                    $("#deleteJourneyDialogConfirmation").dialog("close");
					                    $("#errorDialog").dialog({
					                        resizable: false,
					                        height: 'auto',
					                        width: 'auto',
					                        modal: true,
					                        title: 'Dependent Details',
					                        buttons: {
					                            Ok: function () {
					                                $(this).dialog("close");
					                                jQuery("#JourneyTable").trigger("reloadGrid");
					                            }
					                        }
					                    }); //end dialog
					                }
					                else {
					                    $("#deleteJourneyDialogConfirmation").dialog("close");
					                    $("#errorDialog").dialog({
					                        resizable: false,
					                        height: 'auto',
					                        width: 'auto',
					                        modal: true,
					                        title: 'Journey Details',
					                        buttons: {
					                            Ok: function () {
					                                $(this).dialog("close");
					                                jQuery("#JourneyTable").trigger("reloadGrid");
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
    $('#deleteJourneyDialogConfirmation').dialog('open');
}

function AddJourneyDetails() {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    //    $('#addTravelsJourneyDetails #JourneyDetail_TravelID').val('');
    //    $('#addTravelsJourneyDetails #JourneyDetail_JourneyID').val('');
    //    $('#addTravelsJourneyDetails #JourneyDetail_FromPlace').val('');
    //    $('#addTravelsJourneyDetails #JourneyDetail_ToPlace').val('');
    //    $('#addTravelsJourneyDetails #JourneyDetail_JourneyDate').val('');
    //    $('#addTravelsJourneyDetails #JourneyDetail_JourneyMode').val('');
    //    $('#addTravelsJourneyDetails #JourneyDetail_AdditionalInformation').val('');
    //    $('#addTravelsJourneyDetails #JourneyDetail_JourneyModeDetails').val('');
    //    $('#addTravelsJourneyDetails #JourneyDetail_JourneyFeedback').val('');
    //    $('#empTicket').val(Object['TicketName']);
    $("#FalseJourneyFileUploadBtn").val("No files selected");
    $("#addTravelsJourneyDetails #empTicket").replaceWith($("#addTravelsJourneyDetails #empTicket").val("").clone(true));
    $('#addJourneyDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Travel Journey"
    });
    $('#addJourneyDialog').dialog('open');
}

function LinkJourneyClick(event, TravelID) {
    var DocumentID = event.id;
    window.location = 'DownloadJourneyFile/Travel?DocumentID=' + DocumentID + '&TravelID=' + TravelID;
    //    window.location = '@Url.Action("DownloadJourneyFile", "Travel")?DocumentID=' + DocumentID + '&TravelID=' + TravelID;
    //    window.location = '@Url.Action("DownloadJourneyFile", "Travel")?DocumentID=' + DocumentID + '&TravelID=' + TravelID;
}

//function LinkJourneyClick(DocumentID, TravelID) {
//    $("#loading").dialog(
//         {
//             closeOnEscape: false,
//             resizable: false,
//             height: 140,
//             width: 300,
//             modal: true,
//             dialogClass: "noclose"
//         });
//    // var DocumentID = event.id;
//    $.ajax({
//        url: "DownloadJourneyFile/Travel",
//        data: { DocumentID: DocumentID, TravelID: TravelID },
//        type: 'GET',
//        success: function (result) {
//            $("#loading").dialog("close");
//            $("#loading").dialog("destroy");
//            $("#showJourneyDetails").empty().append(result);
//
//            $("#showJourneyDetails").dialog({
//                closeOnEscape: false,
//                resizable: false,
//                height: 'auto',
//                width: 'auto',
//                title: "Journey History",
//                buttons: {
//                    "Ok": function () {
//                        $("#loading").dialog("close");
//                        $("#loading").dialog("destroy");
//                        $('#showJourneyDetails').dialog('close');
//                    }
//                },
//                close: function (event, ui) {
//                    $("#loading").dialog("close");
//                    $("#loading").dialog("destroy");
//                    $("#showJourneyDetails").empty();
//                    $("#showJourneyDetails").dialog("destroy");
//                }
//            });
//        }
//    });
//}