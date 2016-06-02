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
 function showDetailsSubmit() {
    $.validator.unobtrusive.parse($("#ShowDetails"));
    if ($("#ShowDetails").valid()) {
        $('#ShowDetails').find('input,select,textarea').removeAttr('disabled');
        var postUrl = "SubmitShowDetailsData/Exit";
        DisplayLoadingDialog(); //checked
        $.ajax({
            url: postUrl,
            type: 'POST',
            cache: false,
            data: $("#ShowDetails").serialize(),
            success: function (data) {
                if (data.status == true) {
                    $("#EmpSeparationShowDetails").dialog("destroy");

                    var MailUrl = "MailTemplate/Exit";
                    var Parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val(), isApproveCall: false, IsRejectCall: false }
                    $.ajax({
                        url: MailUrl,
                        type: 'GET',
                        cache: false,
                        data: Parameter,
                        success: function (data) {
                            if (data) {
                                $("#loading").dialog("close");
                                $("#loading").dialog("destroy");

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
                                        location.reload();
                                    }
                                });
                                $.validator.unobtrusive.parse($("#MailDetails"));
                                $('#sendSeparationMail').click(function () {
                                    $("#CCErrorMessage").hide();
                                    $("#ToErrorMessage").hide();
                                    if ($('#MailDetails').valid()) {
                                        DisplayLoadingDialog(); //checked
                                        var SendMailUrl = "SendEmail/Exit";
                                        $.ajax({
                                            url: SendMailUrl,
                                            type: 'POST',
                                            cache: false,
                                            data: $('#MailDetails').serialize(),
                                            success: function (data) {
                                                $("#loading").dialog("close");
                                                $("#loading").dialog("destroy");

                                                if (data.validCcId == true && data.validtoId == true) {
                                                    if (data.status == true) {
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
                    //$("#loading").dialog("close");
                    //$("#loading").dialog("destroy");
                    $("#ShowDetailsApproveSuccess").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: "Employee Separation Details",
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                            }
                        },
                        close: function () {
                            $(this).dialog('close');
                        }
                    });
                }
                else if (data.status == "Error") {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    $("#EmpSeparationShowDetails").dialog("destroy");
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
                    $("#EmpSeparationShowDetails").dialog("destroy");
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
                $("#EmpSeparationShowDetails").dialog("destroy");
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
    }
}

function ShowDetailsSave(x) {
    if (x == "RMG" || x == "Manager") {
        $('#AgreedReleaseDate').rules("Add", {
            required: true,
            messages:
            {
                required: "Agreed Release Date date is required."
            }
        });
    }
    $.validator.unobtrusive.parse($("#ShowDetails"));
    if ($("#ShowDetails").valid()) {
        $('#ShowDetails').find('input,select,textarea').removeAttr('disabled');
        var postUrl = "SaveShowDetailsData/Exit";
        DisplayLoadingDialog(); //checked
        $.ajax({
            url: postUrl,
            type: 'POST',
            cache: false,
            data: $("#ShowDetails").serialize(),
            success: function (data) {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                if (data.status == true) {
                    $("#EmpSeparationShowDetails").dialog("destroy");
                    $("#ShowDetailsSaveSuccess").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: "Employee Separation Details",
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                            }
                        },
                        close: function () {
                            $(this).dialog('close');
                        }
                    });
                }
                else if (data.status == "Error") {
                    $("#EmpSeparationShowDetails").dialog("destroy");
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
                    $("#EmpSeparationShowDetails").dialog("destroy");
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
    }
}

function ShowdetailsApprove() {
    $.validator.unobtrusive.parse($("#ShowDetails"));
    if ($("#ShowDetails").valid()) {
        $('#ShowDetails').find('input,select,textarea').removeAttr('disabled');
        var postUrl = "ApproveShowDetailsData/Exit";
        DisplayLoadingDialog(); //checked
        $.ajax({
            url: postUrl,
            type: 'POST',
            cache: false,
            data: $("#ShowDetails").serialize(),
            success: function (data) {
                if (data.status == true) {
                    $("#EmpSeparationShowDetails").dialog("destroy");

                    var MailUrl = "MailTemplate/Exit";
                    var Parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val(), isApproveCall: true, IsRejectCall: false }

                    $("#loading").dialog("destroy");
                    $("#ShowDetailsApproveSuccess").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: "Employee Separation Details",
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                                $.ajax({
                                    url: MailUrl,
                                    type: 'GET',
                                    cache: false,
                                    data: Parameter,
                                    success: function (data) {
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
                                                    location.reload();
                                                }
                                            });
                                            $.validator.unobtrusive.parse($("#MailDetails"));
                                            $('#sendSeparationMail').click(function () {
                                                $("#CCErrorMessage").hide();
                                                $("#ToErrorMessage").hide();
                                                if ($('#MailDetails').valid()) {
                                                    DisplayLoadingDialog(); //checked
                                                    var SendMailUrl = "SendEmail/Exit";
                                                    $.ajax({
                                                        url: SendMailUrl,
                                                        type: 'POST',
                                                        cache: false,
                                                        data: $('#MailDetails').serialize(),
                                                        success: function (data) {
                                                            if (data.validCcId == true && data.validtoId == true) {
                                                                if (data.status == true) {
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
                                                            $("#SeparationMailDialog").dialog('close');
                                                            $("#errorDialog").dialog({
                                                                title: 'Mail Error',
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
                    $("#EmpSeparationShowDetails").dialog("destroy");
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
                    $("#EmpSeparationShowDetails").dialog("destroy");
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
                $("#EmpSeparationShowDetails").dialog("destroy");
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
    }
}

function ShowDetailsReject() {
    $('#AgreedReleaseDate').rules("remove");

    $.validator.unobtrusive.parse($("#ShowDetails"));
    if ($("#ShowDetails").valid()) {
        $('#ShowDetails').find('input,select,textarea').removeAttr('disabled');
        var postUrl = "RejectShowDetailsData/Exit";
        DisplayLoadingDialog(); //checked
        $.ajax({
            url: postUrl,
            type: 'POST',
            cache: false,
            data: $("#ShowDetails").serialize(),
            success: function (data) {
                if (data.status == true) {
                    $("#EmpSeparationShowDetails").dialog("destroy");

                    var MailUrl = "MailTemplate/Exit";
                    var Parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val(), isApproveCall: false, IsRejectCall: true }
                    $("#loading").dialog("destroy");
                    $("#ShowDetailsRejectSuccess").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: "Employee Separation Details",
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                                $.ajax({
                                    url: MailUrl,
                                    type: 'GET',
                                    cache: false,
                                    data: Parameter,
                                    success: function (data) {
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
                                                    location.reload();
                                                }
                                            });
                                            $.validator.unobtrusive.parse($("#MailDetails"));
                                            $('#sendSeparationMail').click(function () {
                                                $("#CCErrorMessage").hide();
                                                $("#ToErrorMessage").hide();
                                                if ($('#MailDetails').valid()) {
                                                    DisplayLoadingDialog(); //checked
                                                    var SendMailUrl = "SendEmail/Exit";
                                                    $.ajax({
                                                        url: SendMailUrl,
                                                        type: 'POST',
                                                        cache: false,
                                                        data: $('#MailDetails').serialize(),
                                                        success: function (data) {
                                                            $("#loading").dialog("close");
                                                            $("#loading").dialog("destroy");

                                                            if (data.validCcId == true && data.validtoId == true) {
                                                                if (data.status == true) {
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
                    $("#EmpSeparationShowDetails").dialog("destroy");
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
                    $("#EmpSeparationShowDetails").dialog("destroy");
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
                $("#EmpSeparationShowDetails").dialog("destroy");
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
    }
}