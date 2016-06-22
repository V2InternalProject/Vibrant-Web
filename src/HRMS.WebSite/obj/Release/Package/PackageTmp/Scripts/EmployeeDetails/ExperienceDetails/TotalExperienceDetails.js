/* File Created: August 14, 2013 */
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

//if ($('#UserRole').val() != HRAdmin) {
    //$("#frmAddTotalEmployeeExperienceDetails :input").attr("disabled", true);
    //$('#frmAddTotalEmployeeExperienceDetails #btnSaveEmployeeExperienceDetails').hide();
    //$('#frmAddTotalEmployeeExperienceDetails #btnCancelEmployeeExperienceDetails').hide();
    //$(".ui-datepicker-trigger").hide();

//if (window.userRole != HRAdmin) {
//    $("#frmAddTotalEmployeeExperienceDetails12 #frmAddTotalEmployeeExperienceDetails :input").attr("disabled", true);
//    $('#frmAddTotalEmployeeExperienceDetails12 #btnSaveEmployeeExperienceDetails').hide();
//    $('#frmAddTotalEmployeeExperienceDetails12 #btnCancelEmployeeExperienceDetails').hide();
//}

//if (empStatusId == 2) {
//    $("#frmAddTotalEmployeeExperienceDetails").find('input').attr("disabled", true);
//    $('#frmAddTotalEmployeeExperienceDetails #btnSaveEmployeeExperienceDetails').hide();
//    $('#frmAddTotalEmployeeExperienceDetails #btnCancelEmployeeExperienceDetails').hide();
//}

$(function () {
    $("#frmAddTotalEmployeeExperienceDetails #EmployeeId").val(totalExperinceEmployeeId);
    $('#frmAddTotalEmployeeExperienceDetails').submit(function () {
        if ($(this).valid()) {
            DisplayLoadingDialog(); //checked
            $.ajax({
                url: window.updateTotalDetailUrl,
                type: 'POST',
                data: $('#frmAddTotalEmployeeExperienceDetails').serialize(),
                success: function (results) {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    if (results == true) {
                        $("#AddTotalExperienceDetailsSuccessErrorDialog").dialog({
                            autoOpen: false,
                            resizable: false,
                            height: 140,
                            width: 300,
                            title: "Experience Details",
                            dialogClass: "noclose",
                            modal: true,
                            buttons: {
                                Ok: function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                        $("#AddTotalExperienceDetailsSuccessErrorDialog").html("Experience Details have been updated");

                        $("#AddTotalExperienceDetailsSuccessErrorDialog").dialog('open');
                    } else {
                        $("#AddTotalExperienceDetailsSuccessErrorDialog").dialog({
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
                        $("#AddTotalExperienceDetailsSuccessErrorDialog").html("Experience Details not updated");
                    }
                }
            });
        }
        return false;
    });
});

function cancel()
{
    $("#RelevantExperienceYears").val($("#hdnRelevantExperienceYears").val());
    $("#RelevantExperienceMonths").val($("#hdnRelevantExperienceMonths").val());
}
//$("#btnCancelEmployeeExperienceDetails").click(function () {
//    $("#RelevantExperienceYears").val($("#hdnRelevantExperienceYears").val());
//    $("#RelevantExperienceMonths").val($("#hdnRelevantExperienceMonths").val());
//});