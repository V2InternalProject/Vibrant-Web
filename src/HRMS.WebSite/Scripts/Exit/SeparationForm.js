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
// Resets the value on the form
function RestoreSeparationValues() {
    $("#EmpName").val($("#hdempName").val());
    $("#EmpComment").val($("#hdempComment").val());
    $("#ResignedDate").val($("#hdresignedDate").val());
    $("#NoticePeriod").val($("#hdnoticePeriod").val());
    $("#ReasonForSeparartion").val($("#hdreasonForSeparartion").val());
    $("#TentativeReleaseDate").val($("#hdtentativeReleaseDate").val());
    $("#ModeOfSeparation").val($("#hdForModeOfSeparartion").val());
    $("#SystemReleavingDate").val($("#hdSystemReleavingDate").val());
}

function SaveSeparationForm() {
    if ($("#SeparationForm").valid()) {
        if ($('#Isterminate').val() == "yes") {
            $('#terminationconfirmation').dialog(
			{
			    modal: true,
			    width: 380,
			    height: 160,
			    resizable: false,
			    title: "Termination Confirmation",
			    dialogClass: 'noclose',
			    buttons:
						{
						    "Ok": function () {
						        DisplayLoadingDialog();  //checked
						        $("#loading").dialog('open');
						        $.ajax({
						            url: "SaveSeparationForm/Exit",
						            type: 'POST',
						            cache: false,
						            data: $('#SeparationForm').serialize(),
						            success: function (results) {
						                $("#terminationconfirmation").dialog("destroy");

						                $("#loading").dialog("destroy");

						                if (results.status == true && results.exitId) {
						                    var MailUrl = "MailTemplate/Exit";
						                    var Parameter = { exitInstanceId: results.exitId, isApproveCall: false, IsRejectCall: false, Isterminated: $("#Isterminate").val() }
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
						                                        window.location.href = "SeparationForm/Exit";
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
						                                                        $("#SeparationMailDialog").dialog('close');
						                                                    }
						                                                }
						                                                else if (data.status == "Error") {
						                                                    $("#SeparationMailDialog").dialog('close');
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
						                                                    width: '420',
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
						                                });
						                            }
						                        }
						                    });
						                } //end if status == true

						                else if (results.status == "Error") {
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
						            } //end success
						        });
						    },
						    Cancel: function () {
						        $(this).dialog("close");
						    }
						}
			});
        } //end of isterminated
        else {
            $('#btnSaveSeparation').hide();
            if (IsExitConfManagerSet == "False") {
                $("#ExitConfirmationManagerError").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Separation Process Error',
                    buttons: {
                        Ok: function () {
                            $('#btnSaveSeparation').show();
                            $(this).dialog("close");
                        }
                    }
                });  //end dialog
                return false;
            }

            else {
                DisplayLoadingDialog();  //checked
                $("#loading").dialog('open');
                $.ajax({
                    url: postUrl,
                    type: 'POST',
                    cache: false,
                    data: $('#SeparationForm').serialize(),
                    datatype: 'json',
                    success: function (results) {
                        $("#loading").dialog("destroy");
                        if (results.status == true && results.exitId) {
                            var MailUrl = "MailTemplate/Exit";
                            var Parameter = { exitInstanceId: results.exitId, isApproveCall: false, IsRejectCall: false }
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
                                            closeOnEscape: false,
                                            dialogClass: 'noclose',
                                            title: "Send Mail",
                                            open: function () {
                                                $(this).parent().prev('.ui-widget-overlay').css('z-index', '30');
                                                $(this).parent().css('z-index', '31');
                                            },
                                            close: function () {
                                                window.location.href = "EmpSeparationApprovals";
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
                                                                window.location.href = "EmpSeparationApprovals";
                                                            }
                                                        }
                                                        else if (data.status == "Error") {
                                                            $("#SeparationMailDialog").dialog('close');
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
                                                        else {
                                                            if (data.validCcId == false)
                                                                $("#CCErrorMessage").show();

                                                            if (data.validtoId == false)
                                                                $("#ToErrorMessage").show();
                                                            return false;
                                                        }
                                                    },
                                                    error: function () {
                                                        $("#loading").dialog("destroy");
                                                        $("#SeparationMailDialog").dialog('close');
                                                        $("#errorDialog").dialog({
                                                            title: 'Mail Error',
                                                            resizable: false,
                                                            height: 'auto',
                                                            width: '420',
                                                            modal: true,
                                                            buttons: {
                                                                Ok: function () {
                                                                    $(this).dialog("close");
                                                                }
                                                            }
                                                        }); //end dialog
                                                        window.location.href = "EmpSeparationApprovals";
                                                    }
                                                });
                                            }
                                        });
                                    }
                                }
                            });
                        } //end if status==true

                        else if (results.status == "Error") {
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
                    } //end success
                });
            }
        }
    }
    return false;
}