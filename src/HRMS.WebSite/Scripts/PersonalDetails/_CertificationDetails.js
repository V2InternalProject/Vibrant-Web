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
function DeleteCertificationDetail(selectedId, certificationEmployeeId) {
    $('#DeleteConfirmationDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 300,
        height: 'auto',
        resizable: false,
        dialogClass: "noclose",
        title: "Delete Certification Detail",
        buttons:
		        {
		            "Ok": function () {
		                $.ajax({
		                    url: "DeleteCertificationDetails/PersonalDetails",
		                    data: { employeeCertificationID: selectedId, certificationEmployeeId: certificationEmployeeId },
		                    success: function (data) {
		                        if (data.status == true) {
		                            $("#DeleteConfirmationDialog").dialog("close");
		                            $("#DeleteConfirmation").dialog({
		                                modal: true,
		                                resizable: false,
		                                height: 140,
		                                width: 300,
		                                title: "Deleted",
		                                dialogClass: "noclose",
		                                buttons:
		                                {
		                                    "Ok": function () {
		                                        jQuery("#certificationTable").trigger("reloadGrid");
		                                        $(this).dialog('close');
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
		                                title: 'Certification Details',
		                                dialogClass: "noclose",
		                                buttons: {
		                                    Ok: function () {
		                                        $(this).dialog("close");
		                                        jQuery("#certificationTable").trigger("reloadGrid");
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
		                                title: 'Certification Details',
		                                dialogClass: "noclose",
		                                buttons: {
		                                    Ok: function () {
		                                        $(this).dialog("close");
		                                        jQuery("#certificationTable").trigger("reloadGrid");
		                                    }
		                                }
		                            }); //end dialog
		                        }
		                    }
		                });
		            },
		            "Cancel": function () {
		                $(this).dialog('close');
		            } /// <reference path="../../Controllers/OrbitController.cs" />
		        }
    });
    $('#DeleteConfirmationDialog').dialog('open');
}

function EditCertificationDetails(Object) {
    $("#certificationDialog #NewCertification_EmployeeCertificationID").val(Object['EmployeeCertificationID']);
    $("#certificationDialog #NewCertification_CertificationName option[value = " + Object['CertificationNameID'] + "] ").attr('selected', 'selected');
    $('#certificationDialog #NewCertification_CertificationNo').val(Object['CertificationNo']);
    $('#certificationDialog #NewCertification_Institution').val(Object['Institution']);
    $('#certificationDialog #NewCertification_CertificationDate').val(Object['CertificationDate']);
    $('#certificationDialog #NewCertification_CertificationScore').val(Object['CertificationScore']);
    $('#certificationDialog #NewCertification_CertificationGrade').val(Object['CertificationGrade']);
    $('#certificationDialog #certificationName').val(Object['CertificationNameID']);
    $('#certificationDialog #certificationNo').val(Object['CertificationNo']);
    $('#certificationDialog #institution').val(Object['Institution']);
    $('#certificationDialog #certificationDate').val(Object['CertificationDate']);
    $('#certificationDialog #certificationScore').val(Object['CertificationScore']);
    $('#certificationDialog #certificationGrade').val(Object['CertificationGrade']);
    $('#certificationDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 490,
        resizable: false,
        title: "Edit Certification Details"
    }
        );
    $('#certificationDialog').dialog('open');
}

$('#btnAddCertificationDetails').click(function () {
    $("#certificationDialog #NewCertification_EmployeeCertificationID").val('');
    $('#certificationDialog #NewCertification_CertificationName').val('');
    $('#certificationDialog #NewCertification_CertificationNo').val('');
    $('#certificationDialog #NewCertification_Institution').val('');
    $('#certificationDialog #NewCertification_CertificationDate').val('');
    $('#certificationDialog #NewCertification_CertificationScore').val('');
    $('#certificationDialog #NewCertification_CertificationGrade').val('');
    $('#certificationDialog #certificationName').val('');
    $('#certificationDialog #certificationNo').val('');
    $('#certificationDialog #institution').val('');
    $('#certificationDialog #certificationDate').val('');
    $('#certificationDialog #certificationScore').val('');
    $('#certificationDialog #certificationGrade').val('');
    $('#certificationDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 490,
        resizable: false,
        title: "Add Certification Details"
    }
        );
    $('#certificationDialog').dialog('open');
});

var emptyDialog = function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#NewCertification_CertificationName").val($('#certificationName').val());
    $("#NewCertification_CertificationDate").val($('#certificationDate').val());
    $("#NewCertification_CertificationNo").val($('#certificationNo').val());
    $("#NewCertification_Institution").val($('#institution').val());
    $("#NewCertification_CertificationScore").val($('#certificationScore').val());
    $("#NewCertification_CertificationGrade").val($('#certificationGrade').val());
}

function CallforSendMail(certificationEmployeeID) {
    DisplayLoadingDialog(); //checked
    $.ajax({
        url: "MailSend/PersonalDetails",
        type: 'POST',
        async: false,
        data: { employeeId: certificationEmployeeID, Module: "Certification Details" },
        success: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            //$("#AddCertificationSuccessMessege").dialog('destroy');
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
                    dialogClass: 'noclose',
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
        error: function () {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#AddCertificationSuccessMessege").dialog('destroy');
            $("#mailError").dialog({
                resizable: false,
                height: 'auto',
                width: 'auto',
                modal: true,
                title: 'Mail Error',
                dialogClass: 'noclose',
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    });
}

function isCertificateSelected(value, colname) {
    if (value == "0") {
        $("#CertificateRequiredDialog").dialog({
            modal: true,
            resizable: false,
            height: 140,
            width: 300,
            dialogClass: "noclose",
            buttons:
            {
                "Ok": function () {
                    $(this).dialog("close");
                }
            }
        });
        $.preventDefault();
    }
    else {
        return [true, ""];
    }
}//end function