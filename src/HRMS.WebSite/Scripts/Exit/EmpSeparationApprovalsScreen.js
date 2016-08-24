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
function ValidateCurrentForm(selector) {
    var errorCount = 0;
    $(selector).each(function (index, row) {
        if ($(row).find(".textAeraClass").length) {
            //			if ($.trim($(row).find(".textAeraClass").val()) == "") {
            //				$(row).find("#errorComment").text("Please enter comment").css("display", "");
            //				errorCount += 1;
            //			}
            if (!($(row).find("input[class='radioControl']:checked").val())) {
                $(row).find("#errorRadio").text("Please select option").css("display", "");
                errorCount += 1;
            }
        }
    });
    return errorCount;
}

function renderImages(cellvalue, options, rowobject) {
    var obj;
    obj = "";
    for (var i = 1; i < 8; i++) {
        if (rowobject['ExitStageOrder'] >= i) {
            if ((rowobject['Field'] == "Reject" && rowobject['ExitStageOrder'] == i) || (rowobject['Field'] == "Push Back" && rowobject['ExitStageOrder'] == i)) {
                obj = obj + "<img src='../../Images/New Design/status-rejected.png' width='31px' height='31px' class='StatusImagesRed'> "; //  Red
            } else if (rowobject['ExitStageOrder'] != i) {
                obj = obj + "<img src='../../Images/New Design/status-on.png' width='31px' height='31px' class='StatusImagesGreen'> "; // Green
            }
            else {
                obj = obj + "<img src='../../Images/New Design/status-off.png' width='31px' height='31px' class='StatusImagesGray'>"; //  Yellow
                if (rowobject['StageID'] == 7) {
                    obj = obj + "<img src='../../Images/New Design/status-on.png' width='31px' height='31px' class='StatusImagesGreen'> "; // Green
                }
            }
        } else {
            obj = obj + "<img src='../../Images/New Design/status-off.png' width='31px' height='31px' class='StatusImagesGray'>"; //  Yellow
        }
    }

    return obj;
}

function formatlink(cellvalue, options, rowobject) {
    return "<a href=# id=" + rowobject['ExitInstanceId'] + " class=EmpLink onClick = LinkClick(this," + "\"" + rowobject['EncryptedExitInstanceId'] + "\"," + rowobject['EmployeeId'] + "," + rowobject['ReportingTo'] + "," + rowobject['StageId'] + "," + rowobject['IsWithdrawn'] + ") class=EmployeeNameLink >" + cellvalue + "</a>";
}

function WithdrawResign() {
    $("#WithdrawConfirmationDialog").dialog({
        title: 'Withdraw Resignation',
        resizable: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        dialogClass: "noclose",
        buttons: {
            Yes: function () {
                var Url = "WithdrawEmployeeResignation/Exit";
                var Parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val() };

                $(this).dialog('close');
                DisplayLoadingDialog(); //checked

                $.ajax({
                    url: Url,
                    type: 'GET',
                    cache: false,
                    data: Parameter,
                    success: function (data) {
                        if (data.status == true) {
                            var MailUrl = "MailTemplate/Exit";
                            var Parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val(), isApproveCall: false, IsRejectCall: false }
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
                                                        $("#loading").dialog("destroy");

                                                        if (data.validCcId == true && data.validtoId == true) {
                                                            if (data.status == true) {
                                                                $("#SeparationMailDialog").dialog('close');
                                                            }
                                                        } else if (data.status == "Error") {
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
                                                        } else {
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

                            $("#loading").dialog("destroy");
                        } //end if status == true
                        else if (data.status == "Error") {
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
                        } else {
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
    });  //end dialog
}

function ShowStatusResign() {
    jQuery("#ShowSeparationStatusTable").trigger("reloadGrid");
    $("#EmpSeparationShowStatus").dialog({
        title: 'Employee Separation Process Summary',
        resizable: false,
        height: 'auto',
        width: 1210,
        open: function () {
            $(this).parent().prev('.ui-widget-overlay').css('z-index', '26');
            $(this).parent().css('z-index', '27');
        },
        modal: true
    }); //end dialog
}

function SubmitResignation() {
    var postUrl = "GetSeparationShowDetails/Exit";
    var Parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val() };
    DisplayLoadingDialog(); //checked
    $.ajax({
        url: postUrl,
        type: 'GET',

        cache: false,
        data: Parameter,
        success: function (data) {
            if (data) {
                $("#loading").dialog("destroy");
                $("#EmpSeparationShowDetails").html(data);
                $("#EmpSeparationShowDetails").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 1210,
                    modal: true,
                    title: "Employee Separation Details",
                    close: function () {
                        $(this).dialog('close');
                    }
                });
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
}

function ShowDetailsResign() {
    var postUrl = "GetSeparationShowDetails/Exit";
    var Parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val() };
    DisplayLoadingDialog();  //checked
    $.ajax({
        url: postUrl,
        type: 'GET',
        cache: false,
        data: Parameter,
        success: function (data) {
            if (data) {
                $("#loading").dialog("destroy");
                $("#EmpSeparationShowDetails").html(data);
                $("#EmpSeparationShowDetails").dialog({
                    resizable: false,
                    height: 600,
                    width: 1210,
                    modal: true,
                    title: "Employee Separation Details",
                    close: function () {
                        $(this).dialog('close');
                    }
                });
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
}

function ExitInterviewForm() {
    var postUrl = "GetExitInterviewFormDetails/Exit";
    var Parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val() };
    DisplayLoadingDialog();  //checked
    $.ajax({
        url: postUrl,
        type: 'GET',
        cache: false,
        data: Parameter,
        success: function (data) {
            if (data) {
                $("#loading").dialog("destroy");
                $("#ExitInterviewFromDialog").html(data);
                $("#ExitInterviewFromDialog").dialog({
                    resizable: false,
                    height: '600',
                    width: 1210,
                    modal: true,
                    title: "Employee Separation Details",
                    close: function () {
                        $(this).dialog('close');
                    }
                });
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
}

function HRCommentsDetailsForm() {
    var postUrl = "GetExitInterviewFormDetails/Exit";
    var Parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val() };
    DisplayLoadingDialog(); //checked
    $.ajax({
        url: postUrl,
        type: 'GET',
        cache: false,
        data: Parameter,
        success: function (data) {
            if (data) {
                $("#loading").dialog("destroy");
                $("#ExitInterviewFromDialog").html(data);
                $("#ExitInterviewFromDialog").dialog({
                    resizable: false,
                    height: '600',
                    width: '1000',
                    modal: true,
                    title: "Employee Separation Details",
                    close: function () {
                        $(this).dialog('close');
                    }
                });
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
}

function FieldDDLChange() {
    if ($("#Field").val() == "Select") {
        $('#FieldChildListExitBG').hide();
        $('#FieldChildListExitOU').hide();
        $('#FieldChildListExitSN').hide();
        }
    if ($("#Field").val() != "") {
        if ($("#Field").val() == "Business Group") {
            $('#FieldChildListExitBG').show();
            $('#FieldChildListExitOU').hide();
            $('#FieldChildListExitSN').hide();
        }
        if ($("#Field").val() == "Organization Unit") {
            $('#FieldChildListExitBG').hide();
            $('#FieldChildListExitOU').show();
            $('#FieldChildListExitSN').hide();
        }
        if ($("#Field").val() == "Stage Name") {
            $('#FieldChildListExitBG').hide();
            $('#FieldChildListExitOU').hide();
            $('#FieldChildListExitSN').show();
        }
    } else {
        $("#Field").val("");
        $("#Field").hide();
    }
}

function AcceptRejectResign() {
    var postUrl = "GetSeparationShowDetails/Exit";
    var parameter = { exitInstanceId: $("#exitIdToPassOnLinkClick").val() };
    DisplayLoadingDialog();  //checked
    $.ajax({
        url: postUrl,
        type: 'GET',
        cache: false,
        data: parameter,
        success: function (data) {
            $("#loading").dialog("destroy");
            if (data) {
                $("#EmpSeparationShowDetails").html(data);
                $("#EmpSeparationShowDetails").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 1210,
                    modal: true,
                    title: "Employee Separation Details",
                    close: function () {
                        $(this).dialog('close');
                    }
                });
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
}

function ViewCheckList() {
    if ($("#UserRole").val() == "HR Admin" && ($("#loginUsersDepartment").val() == "HR CLEARANCE" || $("#LoggedInUser").val() == $("#hdnReportingTo").val())) {
        $("#Adminclrearance").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#Adminclrearance").load("/Exit/DepartmentFormForAdmin?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    else if ($("#loginUsersDepartment").val() == "IT CLEARANCE") {
        $("#dialog1_ITDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_ITDept").load("/Exit/ITClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    else if ($("#loginUsersDepartment").val() == "FINANCE CLEARANCE") {
        $("#dialog1_FinanceDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_FinanceDept").load("/Exit/FinanceClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    else if ($("#loginUsersDepartment").val() == "HR CLEARANCE") {
        $("#dialog1_HRDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_HRDept").load("/Exit/HRClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    else if ($("#loginUsersDepartment").val() == "ADMIN CLEARANCE") {
        $("#dialog1_ADMINDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_ADMINDept").load("/Exit/ADMINClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    else if ($("#loginUsersDepartment").val() == "ASSET MANAGEMENT CLEARANCE") {
        $("#dialog1_AssetDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_AssetDept").load("/Exit/AssetClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    else if ($("#LoggedInUser").val() == $("#hdnReportingTo").val()) {
        $("#dialog1_ProjectDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_ProjectDept").load("/Exit/ProjectClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    else if ($("#UserRole").val() == "HR Admin") {
        $("#Adminclrearance").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#Adminclrearance").load("/Exit/DepartmentFormForAdmin?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    $("#hdnReportingTo").val('');
}

function FillCheckList() {
    if ($("#loginUsersDepartment").val() == "IT CLEARANCE" && $("#LoggedInUser").val() == $("#hdnReportingTo").val()) {
        $("#ITSelection").dialog({
            autoOpen: false,
            modal: true,
            width: 200,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            buttons: {
                Ok: function () {
                    $("#ITSelection").dialog('close');
                    $("#ITSelection").dialog('destroy');
                    if ($("#radioIT1").is(":checked")) {
                        $("#dialog1_ProjectDept").dialog({
                            modal: true,
                            width: 1210,
                            height: 600,
                            resizable: false,
                            title: "Fill Separation Form (Resign)",
                            open: function (event, ui) {
                                $("#dialog1_ProjectDept").load("/Exit/ProjectClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
                            }
                        });
                    }
                    else {
                        if ($("#radioIT2").is(":checked")) {
                            $("#dialog1_ITDept").dialog({
                                modal: true,
                                width: 1210,
                                height: 600,
                                resizable: false,
                                title: "Fill Separation Form (Resign)",
                                open: function (event, ui) {
                                    $("#dialog1_ITDept").load("/Exit/ITClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
                                }
                            });
                        }
                    }
                }
            }
        });
        $('#HRSelection').dialog('open');
    }
    else if ($("#loginUsersDepartment").val() == "IT CLEARANCE") {
        $("#dialog1_ITDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_ITDept").load("/Exit/ITClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    //Asset Department
    if ($("#loginUsersDepartment").val() == "ASSET MANAGEMENT CLEARANCE" && $("#LoggedInUser").val() == $("#hdnReportingTo").val()) {
        $("#AssetSelection").dialog({
            autoOpen: false,
            modal: true,
            width: 200,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            buttons: {
                Ok: function () {
                    $("#AssetSelection").dialog('close');
                    $("#AssetSelection").dialog('destroy');
                    if ($("#radioAsset1").is(":checked")) {
                        $("#dialog1_ProjectDept").dialog({
                            modal: true,
                            width: 1210,
                            height: 600,
                            resizable: false,
                            title: "Fill Separation Form (Resign)",
                            open: function (event, ui) {
                                $("#dialog1_ProjectDept").load("/Exit/ProjectClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
                            }
                        });
                    }
                    else {
                        if ($("#radioAsset2").is(":checked")) {
                            $("#dialog1_AssetDept").dialog({
                                modal: true,
                                width: 1210,
                                height: 600,
                                resizable: false,
                                title: "Fill Separation Form (Resign)",
                                open: function (event, ui) {
                                    $("#dialog1_AssetDept").load("/Exit/AssetClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
                                }
                            });
                        }
                    }
                }
            }
        });
        $('#HRSelection').dialog('open');
    }
    else if ($("#loginUsersDepartment").val() == "ASSET MANAGEMENT CLEARANCE") {
        $("#dialog1_AssetDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_AssetDept").load("/Exit/AssetClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    ///

    else if ($("#loginUsersDepartment").val() == "FINANCE CLEARANCE" && $("#LoggedInUser").val() == $("#hdnReportingTo").val()) {
        $("#FinanceSelection").dialog({
            autoOpen: false,
            modal: true,
            width: 200,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            buttons: {
                Ok: function () {
                    $("#FinanceSelection").dialog('close');
                    $("#FinanceSelection").dialog('destroy');
                    if ($("#radioFinance1").is(":checked")) {
                        $("#dialog1_ProjectDept").dialog({
                            modal: true,
                            width: 1210,
                            height: 600,
                            resizable: false,
                            title: "Fill Separation Form (Resign)",
                            open: function (event, ui) {
                                $("#dialog1_ProjectDept").load("/Exit/ProjectClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
                            }
                        });
                    }
                    else {
                        if ($("#radioFinance2").is(":checked")) {
                            $("#dialog1_FinanceDept").dialog({
                                modal: true,
                                width: 1210,
                                height: 600,
                                resizable: false,
                                title: "Fill Separation Form (Resign)",
                                open: function (event, ui) {
                                    $("#dialog1_FinanceDept").load("/Exit/FinanceClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
                                }
                            });
                        }
                    }
                }
            }
        });
        $('#HRSelection').dialog('open');
    }
    else if ($("#loginUsersDepartment").val() == "FINANCE CLEARANCE") {
        $("#dialog1_FinanceDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_FinanceDept").load("/Exit/FinanceClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    else if ($("#loginUsersDepartment").val() == "HR CLEARANCE" && $("#LoggedInUser").val() == $("#hdnReportingTo").val()) {
        $("#HRSelection").dialog({
            autoOpen: false,
            modal: true,
            width: 300,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            buttons: {
                Ok: function () {
                    $("#HRSelection").dialog('close');
                    $("#HRSelection").dialog('destroy');
                    if ($("#radioHR1").is(":checked")) {
                        $("#dialog1_ProjectDept").dialog({
                            modal: true,
                            width: 1210,
                            height: 600,
                            resizable: false,
                            title: "Fill Separation Form (Resign)",
                            open: function (event, ui) {
                                $("#dialog1_ProjectDept").load("/Exit/ProjectClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
                            }
                        });
                    }
                    else {
                        if ($("#radioHR2").is(":checked")) {
                            $("#dialog1_HRDept").dialog({
                                modal: true,
                                width: 1210,
                                height: 600,
                                resizable: false,
                                title: "Fill Separation Form (Resign)",
                                open: function (event, ui) {
                                    $("#dialog1_HRDept").load("/Exit/HRClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
                                }
                            });
                        }
                    }
                }
            }
        });
        $('#HRSelection').dialog('open');
    }
    else if ($("#loginUsersDepartment").val() == "HR CLEARANCE") {
        $("#dialog1_HRDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_HRDept").load("/Exit/HRClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    else if ($("#loginUsersDepartment").val() == "ADMIN CLEARANCE" && $("#LoggedInUser").val() == $("#hdnReportingTo").val()) {
        $("#AdminSelection").dialog({
            autoOpen: true,
            modal: true,
            width: 200,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            buttons: {
                Ok: function () {
                    $("#AdminSelection").dialog('close');
                    $("#AdminSelection").dialog('destroy');
                    if ($("#radioAdmin1").is(":checked")) {
                        $("#dialog1_ProjectDept").dialog({
                            modal: true,
                            width: 1210,
                            height: 600,
                            resizable: false,
                            title: "Fill Separation Form (Resign)",
                            open: function (event, ui) {
                                $("#dialog1_ProjectDept").load("/Exit/ProjectClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
                            }
                        });
                    }
                    else {
                        if ($("#radioAdmin2").is(":checked")) {
                            $("#dialog1_ADMINDept").dialog({
                                modal: true,
                                width: 1210,
                                height: 600,
                                resizable: false,
                                title: "Fill Separation Form (Resign)",
                                open: function (event, ui) {
                                    $("#dialog1_ADMINDept").load("/Exit/ADMINClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
                                }
                            });
                        }
                    }
                }
            }
        });
        //        $('#HRSelection').dialog('open');
    }
    else if ($("#loginUsersDepartment").val() == "ADMIN CLEARANCE") {
        $("#dialog1_ADMINDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_ADMINDept").load("/Exit/ADMINClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    else if ($("#LoggedInUser").val() == $("#hdnReportingTo").val()) {
        $("#dialog1_ProjectDept").dialog({
            modal: true,
            width: 1210,
            height: 600,
            resizable: false,
            title: "Fill Separation Form (Resign)",
            open: function (event, ui) {
                $("#dialog1_ProjectDept").load("/Exit/ProjectClearance?exitInstanceId=" + $("#exitIdToPassOnLinkClick").val()).dialog("open");
            }
        });
    }
    $("#hdnReportingTo").val('');
}