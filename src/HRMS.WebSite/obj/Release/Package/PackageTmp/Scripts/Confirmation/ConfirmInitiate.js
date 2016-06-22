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
$("#ReportngMgr2ErrorMessage").hide();
$("#ReviewerErrorMessage").hide();
$("#ReportngMgrErrorMessage").hide();

$("#ReportingManager").change(function () {
    if ($('#ReportingManager').val() == $('#Reviewer').val() || $('#ReportingManager').val() == $('#ReportingManager2').val()) {
        $("#ReportngMgrErrorMessage").show();
        return false;
    }
    else {
        $("#ReportngMgrErrorMessage").hide();
    }
});

$("#Reviewer").change(function () {
    if ($('#Reviewer').val() == $('#ReportingManager').val() || $('#Reviewer').val() == $('#ReportingManager2').val()) {
        $("#ReviewerErrorMessage").show();
        return false;
    }
    else {
        $("#ReviewerErrorMessage").hide();
    }
});

$("#ReportingManager2").change(function () {
    if (($('#ReportingManager2').val() == $('#Reviewer').val() || $('#ReportingManager2').val() == $('#ReportingManager').val()) && ($('#ReportingManager2').val() != "")) {
        $("#ReportngMgr2ErrorMessage").show();
        return false;
    }
    else {
        $("#ReportngMgr2ErrorMessage").hide();
    }
});

$("#frmAddEmployeeDisciplines #Manager").autocomplete({
    source: function (request, response) {
        $.getJSON('SearchEmployeeAutoSuggest/EmployeeDetails', { term: request.term }, function (data) {
            response($.map(data, function (el) {
                var emplyeeInformation = el.EmployeeName;
                return {
                    label: emplyeeInformation,
                    value: el.EmployeeName
                };
            }));
        });
    }
});

//$('#btnInitiate').off('click').on('click', function () {
//    if (($('#ReportingManager').val() != "" && $('#Reviewer').val() != "" && $('#ReportingManager2').val() != "") && ($('#ReportingManager').val() == $('#Reviewer').val() || $('#ReportingManager').val() == $('#ReportingManager2').val() || $('#Reviewer').val() == $('#ReportingManager2').val())) {
//        if ($('#Reviewer').val() == $('#ReportingManager').val() || $('#Reviewer').val() == $('#ReportingManager2').val()) {
//            $("#ReviewerErrorMessage").show();
//            return false;
//        }
//        if ($('#Reviewer').val() == $('#ReportingManager').val() || $('#Reviewer').val() == $('#ReportingManager2').val()) {
//            $("#ReviewerErrorMessage").show();
//            return false;
//        }
//        if (($('#ReportingManager2').val() == $('#Reviewer').val() || $('#ReportingManager2').val() == $('#ReportingManager').val()) && ($('#ReportingManager2').val() != "")) {
//            $("#ReportngMgr2ErrorMessage").show();
//            return false;
//        }
//    }
//    else {
//        var postUrl = 'SaveInitiateConfirmationDetails/ConfirmationProcess';
//        $("#ReportngMgrErrorMessage").hide();
//        $("#ReviewerErrorMessage").hide();
//        $("#ReportngMgr2ErrorMessage").hide();
//        if ($('#frmInitiateConfirmation').valid()) {
//            DisplayLoadingDialog();  //checked
//            $.ajax({
//                url: postUrl,
//                type: 'POST',
//                data: $('#frmInitiateConfirmation').serialize(),
//                success: function (results) {
//                    $("#loading").dialog("close");
//                    $("#loading").dialog("destroy");
//                    if (results.status == true) {
//                        $('#ConfirmInitiate').dialog("close");
//                        jQuery("#ConfigurationTable").trigger("reloadGrid");
//                        var mailurl = 'GetEmailTemplate/ConfirmationProcess';
//                        var empid = { employeeId: $("#encryptedInitiatedEmployeeId").val() };
//                        $.ajax({
//                            url: mailurl,
//                            type: 'GET',
//                            data: { employeeId: $("#encryptedInitiatedEmployeeId").val(), isApprReject: "", IsAcceptExtendPIP: "" },
//                            success: function (data) {
//                                $('#MailTemplateDialog').html(data);
//                                $("#MailTemplateDialog").dialog({
//                                    resizable: false,
//                                    height: 520,
//                                    width: 800,
//                                    modal: true,
//                                    title: 'Send Mail'
//                                });

//                                $.validator.unobtrusive.parse($("#addMailTemplate"));
//                                $('#sendInitiateMail').click(function () {
//                                    $("#CCErrorMessage").hide();
//                                    $("#ToErrorMessage").hide();
//                                    if ($('#addMailTemplate').valid()) {
//                                        DisplayLoadingDialog(); //checked

//                                        var SendMailUrl = 'SendEmail/ConfirmationProcess';
//                                        $.ajax({
//                                            url: SendMailUrl,
//                                            type: 'POST',
//                                            data: $('#addMailTemplate').serialize(),
//                                            success: function (data) {
//                                                $("#loading").dialog("close");
//                                                $("#loading").dialog("destroy");
//                                                if (data.validCcId == true && data.validtoId == true) {
//                                                    if (data.status == true) {
//                                                        $("#MailTemplateDialog").dialog('close');
//                                                        $("#mailSuccessMessage").dialog({
//                                                            closeOnEscape: false,
//                                                            resizable: false,
//                                                            height: 140,
//                                                            width: 300,
//                                                            modal: true,
//                                                            title: 'Mail Process',
//                                                            buttons: {
//                                                                Ok: function () {
//                                                                    $(this).dialog("close");
//                                                                }
//                                                            }
//                                                        });
//                                                    }
//                                                    else if (data.status == false) {
//                                                        $("#MailIDError").dialog({
//                                                            title: 'Mail Error',
//                                                            resizable: false,
//                                                            height: 'auto',
//                                                            width: 'auto',
//                                                            modal: true,
//                                                            buttons: {
//                                                                Ok: function () {
//                                                                    $(this).dialog("close");
//                                                                }
//                                                            },
//                                                            close: function () {
//                                                                $(this).dialog("destroy");
//                                                            }
//                                                        }); //end dialog
//                                                    }
//                                                }
//                                                else if (data.status == "Error") {
//                                                    $("#errorDialog").dialog({
//                                                        title: 'Mail Error',
//                                                        resizable: false,
//                                                        height: 'auto',
//                                                        width: 'auto',
//                                                        modal: true,
//                                                        buttons: {
//                                                            Ok: function () {
//                                                                $(this).dialog("close");
//                                                            }
//                                                        }
//                                                    }); //end dialog
//                                                }
//                                                else {
//                                                    if (data.validCcId == false)
//                                                        $("#CCErrorMessage").show();
//                                                    if (data.validtoId == false)
//                                                        $("#ToErrorMessage").show();
//                                                    return false;
//                                                }
//                                            },
//                                            error: function () {
//                                                $("#MailTemplateDialog").dialog('close');
//                                                $("#errorDialog").dialog({
//                                                    title: 'Mail Error',
//                                                    resizable: false,
//                                                    height: 'auto',
//                                                    width: 'auto',
//                                                    modal: true,
//                                                    buttons: {
//                                                        Ok: function () {
//                                                            $(this).dialog("close");
//                                                        }
//                                                    }
//                                                }); //end dialog
//                                            }
//                                        });
//                                    }
//                                });
//                            }
//                        });
//                    } //end if true
//                    else if (results.status == "Error") {
//                        $("#errorDialog").dialog({
//                            title: 'Confirmation Process',
//                            resizable: false,
//                            height: 'auto',
//                            width: 'auto',
//                            modal: true,
//                            buttons: {
//                                Ok: function () {
//                                    $(this).dialog("close");
//                                }
//                            }
//                        }); //end dialog
//                    }
//                    else {
//                        $("#errorDialog").dialog({
//                            title: 'Confirmation Process',
//                            resizable: false,
//                            height: 'auto',
//                            width: 'auto',
//                            modal: true,
//                            buttons: {
//                                Ok: function () {
//                                    $(this).dialog("close");
//                                }
//                            }
//                        }); //end dialog
//                    }
//                } //end success
//            });
//        }
//        return false;
//    }

//});