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
 function SendMailForSkill(skillemployeeID) {
    DisplayLoadingDialog(); //checked
    $.ajax({
        url: "MailSend/PersonalDetails",
        type: 'POST',
        async: false,
        data: { employeeId: skillemployeeID, Module: "Skill Details" },
        success: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#skillSuccessDialog").dialog('destroy');
            if (data.validCcId == true && data.validtoId == true) {
                if (data.status == true) {
                    $("#mailSendSuccess").dialog({
                        closeOnEscape: false,
                        resizable: false,
                        height: 'auto',
                        title: "Mail Sent",
                        width: 300,
                        modal: true,
                        dialogClass: 'noclose',
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                                //window.location.reload();
                            }
                        }
                    });
                }
            }
            else if (data.status == "Error") {
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Mail Error',
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                }); //end dialog
            }
            else if (data.status == "ErrorRecipient") {
                $("#failedRecipient #span_failedRecipient").append(data.failedRecipient);
                $("#failedRecipient").dialog({
                    closeOnEscape: false,
                    resizable: false,
                    height: 'auto',
                    width: 300,
                    modal: true,
                    dialogClass: 'noclose',
                    buttons: {
                        Ok: function () {
                            $(this).dialog('close');
                        }
                    }
                });
            }
            else {
                if (data.validCcId == false) {
                    $("#InvalidEmail").dialog({
                        closeOnEscape: false,
                        resizable: false,
                        height: 'auto',
                        width: 300,
                        modal: true,
                        dialogClass: 'noclose',
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                            }
                        }
                    });
                }
                if (data.validtoId == false) {
                    $("#InvalidEmail").dialog({
                        closeOnEscape: false,
                        resizable: false,
                        height: 'auto',
                        width: 300,
                        modal: true,
                        dialogClass: 'noclose',
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                            }
                        }
                    });
                }
                return false;
            }
        },
        error: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#skillSuccessDialog").dialog('destroy');
            $("#mailError").dialog({
                resizable: false,
                height: 'auto',
                width: 'auto',
                modal: true,
                title: 'Mail Error',
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    });
}