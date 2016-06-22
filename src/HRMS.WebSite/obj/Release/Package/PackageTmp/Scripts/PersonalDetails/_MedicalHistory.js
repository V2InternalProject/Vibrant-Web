$("#btnAddMedicalDesc").click(function () {
    $("#MedicalHistory_MedicalDescId").val(0);
    $("#MedicalHistory_MedicalDescription").val("");
    $('#dialogAction').dialog({
        modal: true,
        width: 500,
        resizable: false,
        title: "Medical History",
        open: function (event, ui) {
            $("#MedicalHistory_Year").empty();
            filldropdownlist();
        },
        close: function (event, ui) {
            $("#dialogAction").dialog("destroy");
        }
    });
});

function EditMedicalDetails(Object) {
    $("#dialogAction #MedicalHistory_EmployeeId").val(Object['EmployeeId']);
    $("#dialogAction #MedicalHistory_MedicalDescription").val(Object['MedicalDescription']);
    $("#dialogAction #MedicalHistory_MedicalDescId").val(Object['MedicalDescId']);
    $("#dialogAction #MedicalHistory_Year").val(Object['Year']);
    $("#dialogAction #MedicalHistory_year").val(Object['Year']);
    $("#dialogAction #MedicalHistory_medicalDescription").val(Object['MedicalDescription']);
    $('#dialogAction').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Medical History",
        close: function (event, ui) {
            $("#dialogAction").dialog("destroy");
        }
    });
    $('#dialogAction').dialog('open');
}

// To Delete Gridview Records
function DeleteMedicalDetail(deleteId, employeeId) {
    $('#deleteMedicalDialogConfirmation').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: 300,
        height: 'auto',
        title: "Delete Medical History",
        dialogClass: "noclose",
        buttons: {
            "Ok": function () {
                $.ajax({
                    url: "DeleteMedicalHistory/PersonalDetails",
                    data: { MedicalDesc_ID: deleteId, employeeId: employeeId },
                    success: function (data) {
                        if (data.status == true) {
                            $("#deleteMedicalDialogConfirmation").dialog("close");
                            $("#deleteMedicalDialogConfirmation").dialog("destroy");

                            $("div.ui-dialog").each(function (e) {
                                $(this).html('');
                            });
                            jQuery("#jqMedicalTable").trigger("reloadGrid");
                            //$("#deleteMedical").dialog({
                            //    modal: true,
                            //    resizable: false,
                            //    height: 140,
                            //    width: 300,
                            //    title: "Deleted",
                            //    buttons: {
                            //        "Ok": function () {
                            //            jQuery("#jqMedicalTable").trigger("reloadGrid");
                            //            $(this).dialog('close');
                            //        }
                            //    }
                            //});
                        }
                        else if (data.status == "Error") {
                            $("#deleteMedicalDialogConfirmation").dialog("close");
                            $("#deleteMedicalDialogConfirmation").dialog("destroy");
                            $("#errorDialog").dialog({
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                title: 'Medical History',
                                dialogClass: "noclose",
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                        jQuery("#jqMedicalTable").trigger("reloadGrid");
                                    }
                                }
                            }); //end dialog
                        }
                        else {
                            $("#deleteMedicalDialogConfirmation").dialog("close");
                            $("#deleteMedicalDialogConfirmation").dialog("destroy");
                            $("#errorDialog").dialog({
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                title: 'Medical History',
                                dialogClass: "noclose",
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                        jQuery("#jqMedicalTable").trigger("reloadGrid");
                                    }
                                }
                            }); //end dialog
                        }
                    },
                    Error: function () { errorOccured(); }
                });
            },
            "Cancel": function () { $(this).dialog('close'); }
        }
    });
    $('#deleteMedicalDialogConfirmation').dialog('open');
}

function restoreBloodGroup() {
    $("#ddlBloodGroup").val($("#hndSelectedBg").val());
}

var closeDialog = function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $('#MedicalHistory_MedicalDescription').val($('#MedicalHistory_medicalDescription').val());
    $('#MedicalHistory_Year').val($('#MedicalHistory_year').val());
}