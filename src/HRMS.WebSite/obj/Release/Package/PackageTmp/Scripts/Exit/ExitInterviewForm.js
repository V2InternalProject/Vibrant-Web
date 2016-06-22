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
function ExitInterviewSave() {
    if ($("#ExitInterviewForm").valid()) {
        DisplayLoadingDialog();  //checked
        var postUrl = "SaveExitInterviewFormData/Exit";
        var postArray = [];
        for (var i = 0; i < ExitInterviewItemsCount; i++) {
            var jsonData = {
                ItemId: $('#ItemId_' + i).val(),
                //$('#DialogForExitInterView .tableRows:eq('+ i +')').
                ResponseId: $('#DialogForExitInterView .tableRows:eq(' + i + ')').find('input[type=radio]:checked').val(),
                //ResponseId: $("#hdRadio_" + (i + 100)).val(),
                //ResponseId: $("#"+ (i +100)).val(),
                RevisionId: $('#Revision_' + i).val(),
                Comments: $('#Comments_' + i).val(),
                ExitInstanceId: $('#ExitInstanceId_' + i).val(),
                HRClosureComments: $('#HRClosureComments').val()
            };
            postArray.push(jsonData);
        }

        $.ajax({
            url: postUrl,
            type: 'POST',
            //cache: false,
            contentType: 'application/json',
            async: false,
            data: JSON.stringify({ model: postArray }),
            success: function (data) {
                $("#loading").dialog("destroy");
                if (data.status == true) {
                    $("#ExitInterviewFromDialog").dialog("destroy");
                    $("#ExitInterviewSaveSuccessPopUp").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: "Separation Interview Form Details",
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                            }
                        }
                    });
                }
                else if (data.status == "Error") {
                    $("#ExitInterviewFromDialog").dialog("destroy");
                    $("#errorDialog").dialog({
                        title: 'Separation Process',
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    }); //end dialog
                }
                else {
                    $("#ExitInterviewFromDialog").dialog("destroy");
                    $("#errorDialog").dialog({
                        title: 'Separation Process',
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    }); //end dialog
                }
            },
            error: function () {
                $("#loading").dialog("destroy");
                $("#ExitInterviewFromDialog").dialog("destroy");
                $("#errorDialog").dialog({
                    title: 'Separation Process',
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
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
}

function ExitInterViewApprove() {
    if ($("#ExitInterviewForm").valid()) {
        $("#ExitInterviewConfirmmessage").dialog({
            resizable: false,
            height: 'auto',
            //width: 'auto',
            modal: true,
            title: "Separation Interview Form Details",
            dialogClass: "noclose",
            open: function () {
                $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                $(this).parent().css('z-index', '27');
            },
            buttons: {
                Yes: function () {
                    var postUrl = "ApproveExitInterViewFormData/Exit";
                    var postArray = [];
                    for (var i = 0; i < ExitInterviewItemsCount; i++) {
                        var jsonData = {
                            ItemId: $('#ItemId_' + i).val(),
                            ResponseId: $('#DialogForExitInterView .tableRows:eq(' + i + ')').find('input[type=radio]:checked').val(),
                            RevisionId: $('#Revision_' + i).val(),
                            Comments: $('#Comments_' + i).val(),
                            ExitInstanceId: $('#ExitInstanceId_' + i).val()
                        };
                        postArray.push(jsonData);
                    }

                    $("#ExitInterviewConfirmmessage").dialog('destroy');
                    DisplayLoadingDialog();  //checked
                    $.ajax({
                        url: postUrl,
                        type: 'POST',
                        cache: false,
                        contentType: 'application/json',
                        data: JSON.stringify({ model: postArray }),
                        success: function (data) {
                            if (data.status == true) {
                                $("#ExitInterviewFromDialog").dialog("destroy");

                                $("#loading").dialog("close");
                                $("#loading").dialog("destroy");
                                $("#ExitInterviewApproveSuccess").dialog({
                                    resizable: false,
                                    height: 'auto',
                                    width: 'auto',
                                    modal: true,
                                    title: "Employee Separation Details",
                                    dialogClass: "noclose",
                                    buttons: {
                                        Ok: function () {
                                            $(this).dialog('close');
                                            var MailUrl = "MailTemplate/Exit";
                                            var Parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val(), isApproveCall: true, IsRejectCall: false }
                                            $.ajax({
                                                url: MailUrl,
                                                type: 'GET',
                                                cache: false,
                                                data: Parameter,
                                                success: function (data) {
                                                    $("#ExitInterviewApproveSuccess").dialog("destroy");
                                                    if (data) {
                                                        $("#SeparationMailDialog").html(data);
                                                        $("#SeparationMailDialog").dialog({
                                                            resizable: false,
                                                            height: 'auto',
                                                            width: 800,
                                                            modal: true,
                                                            title: "Send Mail",
                                                            open: function () {
                                                                $(this).parent().prev('.ui-widget-overlay').css('z-index', '30');
                                                                $(this).parent().css('z-index', '31');
                                                            },
                                                            close: function () {
                                                                jQuery("#jqInboxTable").trigger("reloadGrid");
                                                                jQuery("#jqWatchListTable").trigger("reloadGrid");
                                                                location.reload();
                                                            }
                                                        });
                                                        $.validator.unobtrusive.parse($("#MailDetails"));
                                                        $('#sendSeparationMail').click(function () {
                                                            $("#CCErrorMessage").hide();
                                                            $("#ToErrorMessage").hide();
                                                            if ($('#MailDetails').valid()) {
                                                                DisplayLoadingDialog();  //checked
                                                                var SendMailUrl = "SendEmail/Exit";
                                                                $.ajax({
                                                                    url: SendMailUrl,
                                                                    type: 'POST',
                                                                    cache: false,
                                                                    data: $('#MailDetails').serialize(),
                                                                    success: function (data) {
                                                                        $("#loading").dialog("destroy");
                                                                        if (data.validCcId == true && data.validtoId == true) {
                                                                            if (data.status == true) {
                                                                                jQuery("#jqInboxTable").trigger("reloadGrid");
                                                                                jQuery("#jqWatchListTable").trigger("reloadGrid");
                                                                                $("#SeparationMailDialog").dialog('close');
                                                                            }
                                                                        }
                                                                        else if (data.status == "Error") {
                                                                            $("#SeparationMailDialog").dialog('close');
                                                                            $("#errorDialog").dialog({
                                                                                title: 'Mail Error',
                                                                                resizable: false,
                                                                                height: 'auto',
                                                                                width: 'auto',
                                                                                modal: true,
                                                                                dialogClass: "noclose",
                                                                                buttons: {
                                                                                    Ok: function () {
                                                                                        $(this).dialog("close");
                                                                                    }
                                                                                }
                                                                            }); //end dialog
                                                                        }
                                                                        else {
                                                                            if (data.validCcId == false)
                                                                                $("#CCErrorMessage").show();

                                                                            if (data.validtoId == false)
                                                                                $("#ToErrorMessage").show();

                                                                            return false;
                                                                        }
                                                                    },
                                                                    error: function () {
                                                                        $("#loading").dialog("close");
                                                                        $("#loading").dialog("destroy");
                                                                        $("#SeparationMailDialog").dialog('close');
                                                                        $("#errorDialog").dialog({
                                                                            title: 'Mail Error',
                                                                            resizable: false,
                                                                            height: 'auto',
                                                                            width: 'auto',
                                                                            modal: true,
                                                                            dialogClass: "noclose",
                                                                            buttons: {
                                                                                Ok: function () {
                                                                                    $(this).dialog("close");
                                                                                }
                                                                            }
                                                                        }); //end dialog
                                                                        window.location.href = "EmpSeparationApprovals/Exit";
                                                                    }
                                                                });
                                                            }
                                                        });
                                                    }
                                                }
                                            });
                                        }
                                    }
                                });
                            }
                            else if (data.status == "Error") {
                                $("#loading").dialog("close");
                                $("#loading").dialog("destroy");
                                $("#ExitInterviewFromDialog").dialog("destroy");
                                $("#errorDialog").dialog({
                                    title: 'Separation Process',
                                    resizable: false,
                                    height: 'auto',
                                    width: 'auto',
                                    modal: true,
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
                                $("#ExitInterviewFromDialog").dialog("destroy");
                                $("#errorDialog").dialog({
                                    title: 'Separation Process',
                                    resizable: false,
                                    height: 'auto',
                                    width: 'auto',
                                    modal: true,
                                    buttons: {
                                        Ok: function () {
                                            $(this).dialog("close");
                                        }
                                    }
                                }); //end dialog
                            }
                        },
                        error: function () {
                            $("#loading").dialog("close");
                            $("#loading").dialog("destroy");
                            $("#ExitInterviewFromDialog").dialog("destroy");
                            $("#errorDialog").dialog({
                                title: 'Separation Process',
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            }); //end dialog
                        }
                    });
                },
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
    }
}

function CompleteExitProcess() {
    if ($("#ExitInterviewForm").valid()) {
        $("#HRClosureConfirmation").dialog({
            resizable: false,
            height: 'auto',
            width: 'auto',
            modal: true,
            title: "Confirmation of Separation Process",
            dialogClass: "noclose",
            open: function () {
                $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                $(this).parent().css('z-index', '27');
            },
            buttons: {
                Yes: function () {
                    var postUrl = "ApproveExitInterViewFormData/Exit";
                    var postArray = [];
                    for (var i = 0; i < ExitInterviewItemsCount; i++) {
                        var jsonData = {
                            ItemId: $('#ItemId_' + i).val(),
                            ResponseId: $('#DialogForExitInterView .tableRows:eq(' + i + ')').find('input[type=radio]:checked').val(),
                            RevisionId: $('#Revision_' + i).val(),
                            Comments: $('#Comments_' + i).val(),
                            ExitInstanceId: $('#ExitInstanceId_' + i).val(),
                            HRClosureComments: $('#HRClosureComments').val()
                        };
                        postArray.push(jsonData);
                    }
                    $(this).dialog('close');
                    DisplayLoadingDialog();  //checked
                    $.ajax({
                        url: postUrl,
                        type: 'POST',
                        cache: false,
                        contentType: 'application/json',
                        data: JSON.stringify({ model: postArray }),
                        success: function (data) {
                            if (data.status == true) {
                                $("#ExitInterviewFromDialog").dialog("destroy");
                                $("#loading").dialog("destroy");
                                $("#HRClosureSuccess").dialog({
                                    resizable: false,
                                    height: 'auto',
                                    width: 'auto',
                                    modal: true,
                                    dialogClass: "noclose",
                                    title: "Employee Separation Details",
                                    buttons: {
                                        Ok: function () {
                                            jQuery("#jqInboxTable").trigger("reloadGrid");
                                            jQuery("#jqWatchListTable").trigger("reloadGrid");
                                            $(this).dialog('close');
                                            location.reload();
                                        }
                                    }
                                });
                            }
                            else if (data.status == "Error") {
                                jQuery("#jqInboxTable").trigger("reloadGrid");
                                jQuery("#jqWatchListTable").trigger("reloadGrid");
                                $("#loading").dialog("destroy");
                                $("#ExitInterviewFromDialog").dialog("destroy");
                                $("#errorDialog").dialog({
                                    title: 'Separation Process',
                                    resizable: false,
                                    height: 'auto',
                                    width: 'auto',
                                    modal: true,
                                    buttons: {
                                        Ok: function () {
                                            $(this).dialog("close");
                                        }
                                    }
                                }); //end dialog
                            }
                            else {
                                $("#loading").dialog("destroy");
                                $("#ExitInterviewFromDialog").dialog("destroy");
                                $("#errorDialog").dialog({
                                    title: 'Separation Process',
                                    resizable: false,
                                    height: 'auto',
                                    width: 'auto',
                                    modal: true,
                                    buttons: {
                                        Ok: function () {
                                            $(this).dialog("close");
                                        }
                                    }
                                }); //end dialog
                            }
                        },
                        error: function () {
                            $("#loading").dialog("destroy");
                            $("#errorDialog").dialog({
                                title: 'Separation Process',
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            }); //end dialog
                        }
                    });
                },
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
    }
}

function ExitPushBackStage() {
    $("#PushBackStageConfirmation").dialog({
        resizable: false,
        height: 'auto',
       // width: 'auto',
        modal: true,
        title: "Confirmation of Separation Process",
        dialogClass: "noclose",
        buttons: {
            Yes: function () {
                var postUrl = "PushBackHRClosure/Exit";
                var Parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val() }
                DisplayLoadingDialog();  //checked
                $.ajax({
                    url: postUrl,
                    type: 'POST',
                    cache: false,
                    data: Parameter,
                    success: function (data) {
                        $("#loading").dialog("destroy");
                        if (data.status == true) {
                            $("#PushBackStageConfirmation").dialog("destroy");
                            $("#ExitInterviewFromDialog").dialog("destroy");
                            jQuery("#jqInboxTable").trigger("reloadGrid");
                            jQuery("#jqWatchListTable").trigger("reloadGrid");
                            $("#ExitInterviewRejectSuccess").dialog({
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                dialogClass: "noclose",
                                title: "Employee Separation Details",
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog('close');
                                        location.reload();
                                    }
                                }
                            });
                        }
                        else if (data.status == "Error") {
                            $("#errorDialog").dialog({
                                title: 'Separation Process',
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                dialogClass: "noclose",
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            }); //end dialog
                        }
                        else {
                            $("#errorDialog").dialog({
                                title: 'Separation Process',
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                dialogClass: "noclose",
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            }); //end dialog
                        }
                    },
                    error: function () {
                        $("#loading").dialog("close");
                        $("#loading").dialog("destroy");
                        $("#errorDialog").dialog({
                            title: 'Separation Process',
                            resizable: false,
                            height: 'auto',
                            width: 'auto',
                            modal: true,
                            dialogClass: "noclose",
                            buttons: {
                                Ok: function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                    }
                });
            },
            Cancel: function () {
                $(this).dialog('close');
            }
        }
    });
}

function Workflow() {
    jQuery("#ShowSeparationStatusTable").trigger("reloadGrid");
    $("#EmpSeparationShowStatus").dialog({
        title: 'Employee Separation Process Summary',
        resizable: false,
        height: 500,
        width: 1210,
        open: function () {
            $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
            $(this).parent().css('z-index', '27');
        },
        modal: true
    });
}