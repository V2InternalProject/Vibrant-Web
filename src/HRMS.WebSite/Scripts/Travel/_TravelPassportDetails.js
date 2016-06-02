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
$('#empPassport').change(function () {
    $('#empPassport').attr('title', $(this).val());
    var file = $('input[type="file"]').val();
    var exts = ['exe'];
    // first check if file field has any value
    if (file) {
        $("#PassportUploadError").text("");
        // split file name at dot
        var get_ext = file.split('.');
        // reverse name to check extension
        get_ext = get_ext.reverse();
        // check file type is valid as given in 'exts' array
        if ($.inArray(get_ext[0].toLowerCase(), exts) == 0) {
            $("#EmpPassportUploadError").dialog({
                title: 'Error',
                resizable: false,
                height: 'auto',
                width: 300,
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");

                        if ($.browser.msie) {
                            $('#empPassport').replaceWith($('#empPassport').clone(true));
                        } else {
                            $('#empPassport').val('');
                        }
                    }
                },
                close: function () {
                    if ($.browser.msie) {
                        $('#empPassport').replaceWith($('#empPassport').clone(true));
                    } else {
                        $('#empPassport').val('');
                    }
                }
            });
        }
    }
});

function LinkPassportClickOpen(event, EmployeeId) {
    $("#loading").dialog(
         {
             closeOnEscape: false,
             resizable: false,
             height: 140,
             width: 300,
             modal: true,
             dialogClass: "noclose"
         });
    var DocumentID = event.id;
    $.ajax({
        url: "showPassportDetails/Travel",
        data: { EmployeeID: EmployeeId, DocumentID: DocumentID },
        type: 'GET',
        success: function (result) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#ShowPassportDetailsTravel").empty().append(result);
            $("#ShowPassportDetailsTravel").dialog({
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                width: 'auto',
                modal: true,
                title: "Passport History",
                dialogClass: "noclose",
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                        $(this).dialog("destroy");
                        //                        $("#loading").dialog("close");
                        //                        $("#loading").dialog("destroy");
                    }
                }
            });
        }
    });
}

//function DeletePassportDocument(DocumentID) {
//    if (DocumentID != 0) {
//        $("#ConfirmPassportDelete").dialog({
//            closeOnEscape: false,
//            resizable: false,
//            height: 'auto',
//            width: 'auto',
//            buttons: {
//                "Ok": function () {
//                    $(this).dialog("close");
//                    $("#loading").dialog(
//                        {
//                            closeOnEscape: false,
//                            resizable: false,
//                            height: 140,
//                            width: 300,
//                            modal: true,
//                            dialogClass: "noclose"
//                        });
//                    $.ajax({
//                        url: "DeletePassportDocument/Travel",
//                        data: { DocumentID: DocumentID },
//                        type: 'POST',
//                        success: function (result) {
//                            $("#loading").dialog("close");
//                            $("#loading").dialog("destroy");
//                            if (result.status == true) {
//                                $(this).dialog("close");
//                                $("#DeletePassportSuccess").dialog({
//                                    closeOnEscape: false,
//                                    resizable: false,
//                                    height: 'auto',
//                                    width: 'auto',
//                                    dialogClass: "noclose",
//                                    buttons: {
//                                        "Ok": function () {
//                                            $(this).dialog("close");
//                                            jQuery("#PassportTable").trigger("reloadGrid");
//                                        }
//                                    }
//                                });
//                            } //if close
//                            else {
//                                $("#loading").dialog("close");
//                                $("#loading").dialog("destroy");
//                                $("#errorDialog").dialog({
//                                    closeOnEscape: false,
//                                    resizable: false,
//                                    height: 'auto',
//                                    width: 425,
//                                    buttons: {
//                                        "Ok": function () {
//                                            $("#errorDialog").dialog("close");
//                                        }
//                                    }
//                                });
//                            }
//                        }, //sucuess end
//                        error: function () {
//                            $("#errorDialog").dialog({
//                                closeOnEscape: false,
//                                resizable: false,
//                                height: 'auto',
//                                width: 425,
//                                buttons: {
//                                    "Ok": function () {
//                                        $("#errorDialog").dialog("close");
//                                    }
//                                }
//                            });
//                        }
//                    }); //ajax close
//                }, //ok
//                "Cancel": function () {
//                    $("#ConfirmPassportDelete").dialog("close");
//                }
//            }
//        });       //close dialog
//    } //end if
//}

function SavePassportDetails(TravelID, EmployeeID) {
    $("#frmPassportDetails #EmployeeID").val(EmployeeID);
    $("#frmPassportDetails").find('input').removeAttr("disabled");
    $("#TravelID").val(TravelID);
    if ($("#empPassport").val() != "") {
        DisplayLoadingDialog(); //checked
        $("#frmPassportDetails").ajaxForm({
            success: function (results) {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                var st = $.parseJSON(results);
                if (st.status == true) {
                    $("#empPassport").replaceWith($("#empPassport").clone(true));
                    //$("#empPassport").val("");
                    $("#frmPassportDetails").find('input').attr("disabled", "disabled");
                    $("#empPassport").removeAttr("disabled", "disabled");
                    $("#passportSave").removeAttr("disabled", "disabled");
                    //$("#passportContinue").removeAttr("disabled", "disabled");
                    $('#documentPassportSuccess').dialog({
                        modal: true,
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        dialogClass: "noclose",
                        buttons: {
                            "OK": function () {
                                $(this).dialog("close");
                                jQuery("#PassportTable").trigger("reloadGrid");
                                $('#empPassport').val('');
                            }
                        }
                    });
                }
                else {
                    $('#errorUploadDocumentDialog').dialog({
                        modal: true,
                        resizable: false,
                        height: 'auto',
                        width: 300,
                        title: "Error",
                        buttons: {
                            "OK": function () { $(this).dialog('close'); }
                        }
                    });
                }
            },
            error: function () {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                $('#errorUploadDocumentDialog').dialog({
                    modal: true,
                    resizable: false,
                    height: 'auto',
                    width: 300,
                    title: "Error",
                    buttons: {
                        "OK": function () { $(this).dialog('close'); }
                    }
                });
            }
        });
    }
    else {
        $("#PassportUploadError").text(" Please select File to upload");
        $("#frmPassportDetails").find('input').attr("disabled", "disabled");
        $("#empPassport").removeAttr("disabled", "disabled");
        $("#passportSave").removeAttr("disabled", "disabled");
        //$("#passportContinue").removeAttr("disabled", "disabled");
        return;
    }
}