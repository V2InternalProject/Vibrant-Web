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
function MoveAheadconfirmDialog_Project() {
    var textAreaCollection = $(".textAeraClass");
    var questionnaireOptionsCollection = [];
    var optionidvariable;
    var data2 = questionsdataProject;
    if ($('#ProjectSeparationForm').valid()) {
        $.each(textAreaCollection, function (i, control) {
            data2[i].Comments = $(control).val();
            optionidvariable = { QuestionnaireOptionID: $(control).parent().find(".hiddenQuestionID").val() };
            questionnaireOptionsCollection.push(optionidvariable);
        });
        var ProjectClarance =
        {
            ExitInstanceId: exitInstanceIdToPassProject,
            QuestionnaireID: ProjectQuestionnaireID,
            QuestionnaireOptions: questionnaireOptionsCollection,  // QuestionnaireOption.QuestionnaireOptionID for selected radio controls
            QuestionnaireQuestions: data2 // Filled with comments
        };
    }
    if ($("#ProjectSeparationForm").valid()) {
        $('#MoveAheadConfirmationProject').dialog({
            autoOpen: false,
            modal: true,
            resizable: false,
            height: 'auto',
            width: 300,
            title: 'Move Ahead Confirmation',
            dialogClass: "noclose",
            open: function () {
                $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                $(this).parent().css('z-index', '27');
            },
            buttons: {
                "Yes": function () {
                    jQuery("#jqInboxTable").trigger("reloadGrid");
                    moveaheadsendmailcallProject();
                },
                "No": function () {
                    $(this).dialog('close');
                }
            }
        });
        $("#MoveAheadConfirmationProject").dialog('open');
    }

    function moveaheadsendmailcallProject() {
        if ($("#ProjectSeparationForm").valid()) {
            var postUrl = "MoveAheadDetailsData/Exit";
            $("#MoveAheadConfirmationProject").dialog('destroy');
            DisplayLoadingDialog(); //checked
            $.ajax({
                url: postUrl,
                type: 'POST',
                cache: false,
                data: JSON.stringify(ProjectClarance),
                contentType: 'application/json',
                dataType: "json",
                success: function (data) {
                    $("#loading").dialog("destroy");
                    if (data.status == true) {
                        jQuery("#jqInboxTable").trigger("reloadGrid");
                        jQuery("#jqWatchListTable").trigger("reloadGrid");
                        var mailUrl = "MoveAheadSendMail/Exit";
                        var parameter = { employeeId: employeeIdToPassProject };
                        $.ajax(
                        {
                            url: mailUrl,
                            type: 'GET',
                            cache: false,
                            data: parameter,
                            success: function (maildata) {
                                if (maildata) {
                                    $('#dialog1_ProjectDept').dialog('destroy');
                                    $("#SeparationMailDialog").html(maildata);
                                    $("#SeparationMailDialog").dialog(
                                        {
                                            resizable: false,
                                            height: 'auto',
                                            width: 800,
                                            modal: true,
                                            title: "Send Mail",
                                            close: function () {
                                                $(this).dialog('destroy');
                                                location.reload();
                                            }
                                        });
                                    $.validator.unobtrusive.parse($("#MailDetails"));
                                    $("#SeparationMailDialog #sendSeparationMail").click(function () {
                                        $("#CCErrorMessage").hide();
                                        $("#ToErrorMessage").hide();
                                        if ($('#MailDetails').valid()) {
                                            $("#loading").dialog(
                                                {
                                                    closeOnEscape: false,
                                                    resizable: false,
                                                    height: 140,
                                                    width: 300,
                                                    modal: true,
                                                    dialogClass: "noclose"
                                                });
                                            var sendMailUrl = "SendEmail/Exit";
                                            $.ajax(
                                                {
                                                    url: sendMailUrl,
                                                    type: 'POST',
                                                    async: true,
                                                    cache: false,
                                                    dataType: "json",
                                                    data: $('#MailDetails').serialize(),
                                                    success: function (data) {
                                                        $("#loading").dialog("destroy");
                                                        if (data.validCcId == true && data.validtoId == true) {
                                                            if (data.status == true) {
                                                                $("#SeparationMailDialog").dialog('destroy');
                                                                window.location.href = "EmpSeparationApprovals/Exit";
                                                            }
                                                        }
                                                        else if (data.status == "Error") {
                                                            $("#SeparationMailDialog").dialog('destroy');
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
                                                        $("#SeparationMailDialog").dialog('destroy');
                                                        $("#errorDialog").dialog({
                                                            title: 'Mail Error',
                                                            resizable: false,
                                                            height: 'auto',
                                                            width: 'auto',
                                                            modal: true,
                                                            buttons:
                                                            {
                                                                Ok: function () {
                                                                    $(this).dialog("close");
                                                                }
                                                            }
                                                        }); //end dialog
                                                    }
                                                }); //end ajax
                                        }
                                    });
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
}

function ProjectPendingItemEmailForm() {
    $('#btnProjectSendPendingItem').dialog({
        autoOpen: false,
        modal: true,
        width: 1100,
        resizable: false,
        title: "Pending Items",
        open: function () {
            $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
            $(this).parent().css('z-index', '27');
        }
    });
    $('#btnProjectSendPendingItem').dialog('open');
}

function WorkFlowProjectClick() {
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
    }); //end dialog
}

function RadioQProjectClick() {
    $(this).parent().find("#errorRadio").text("").css("display", "none");
    $(this).parent().parent().find(".hiddenQuestionID").val($(this).attr("optionid"));
    return;
    var totalChecked = 0;
    var totalRadioGroups = 0;
    var previousName = '';
    $.each($('[name*=radioQ]'), function (index, value) {
        var radioName = $(this).attr('name');
        var radioId = this.id;
        if (previousName != radioName) {
            totalRadioGroups++;
        }
        if ($('#' + radioId).is(':checked')) {
            totalChecked++;
        }
        previousName = radioName;
    });
    var percentage = (totalChecked / totalRadioGroups) * 100;
    $('#progressbar').progressbar('value', percentage);
    $('#percentage').html(percentage.toFixed(2) + "%");
}

function SubmitProjectFormClick(event) {
    if (window.ValidateCurrentForm("#projectSeparationTable tr") == 0) {
        var postUrl = "savefinanceseparationDetails/Exit";
        var textAreaCollection = $(".textAeraClass");
        var questionnaireOptionsCollection = [];
        var optionidvariable;
        var data2 = questionsdataProject;
        DisplayLoadingDialog(); //checked
        $.each(textAreaCollection, function (i, control) {
            data2[i].Comments = $(control).val();
            optionidvariable = { QuestionnaireOptionID: $(control).parent().find(".hiddenQuestionID").val() };
            questionnaireOptionsCollection.push(optionidvariable);
        });
        var ProjectClarance =
        {
            ExitInstanceId: exitInstanceIdToPassProject,
            QuestionnaireID: ProjectQuestionnaireID,
            QuestionnaireOptions: questionnaireOptionsCollection,  // QuestionnaireOption.QuestionnaireOptionID for selected redio controls
            QuestionnaireQuestions: data2 // Filled with comments
        };

        $.ajax({
            url: postUrl,
            type: 'POST',
            cache: false,
            data: JSON.stringify(ProjectClarance),
            contentType: 'application/json',
            success: function (results) {
                $("#loading").dialog("destroy");
                if (results.status == true) {
                    $("#successseparationDialog").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        open: function () {
                            $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                            $(this).parent().css('z-index', '27');
                        },
                        title: "Details Successfully Saved",
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                                $("#dialog1_ProjectDept").dialog('close');
                                location.reload();
                            }
                        }
                    });
                }
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
                        height: 140,
                        width: 300,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            }
        });
        // }
    }
    event.preventDefault();
    return false;
}

function PushBackButtonProjectClick() {
    $("#ExitInstanceId").val(exitInstanceIdToPassProject);
    if ($("#ProjectSeparationForm").valid()) {
        $('#PushBackConfirmationProject').dialog({
            autoOpen: true,
            modal: true,
            resizable: false,
            height: 'auto',
            width: 300,
            title: 'Push Back Confirmation',
            dialogClass: "noclose",
            open: function () {
                $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
                $(this).parent().css('z-index', '27');
            },
            buttons: {
                "Yes": function () {
                    $(this).dialog('close');
                    $("#dialog1_ProjectDept").dialog('close');
                    var postUrl = "PushBackDetailsData/Exit";
                    DisplayLoadingDialog();  //checked
                    $.ajax({
                        url: postUrl,
                        type: 'POST',
                        cache: false,
                        //data: $("#ProjectSeparationForm").serialize(),
                        data: { ExitInstanceId: exitInstanceIdToPassProject },
                        success: function (data) {
                            $("#loading").dialog("destroy");
                            $('#dialog1_Project').dialog('destroy');
                            if (data.status == true) {
                                jQuery("#jqInboxTable").trigger("reloadGrid");
                                $("#ShowDetailsRejectSuccess").dialog({
                                    resizable: false,
                                    height: 'auto',
                                    width: 'auto',
                                    modal: true,
                                    title: "Employee Separation Details",
                                    dialogClass: "noclose",
                                    buttons: {
                                        Ok: function () {
                                            $("#dialog1_ProjectDept").dialog("destroy");
                                            $(this).dialog('close');
                                            $('#PushBackConfirmationProject').dialog('close');
                                            jQuery("#jqInboxTable").trigger("reloadGrid");
                                            window.location.reload();
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
                },
                "No": function () {
                    $(this).dialog('close');
                }
            }
        });
    }
}