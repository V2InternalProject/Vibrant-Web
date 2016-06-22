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
function SaveAppraisalNewYear(postUrl) {
    if ($("#AddEditAppYearFrm").valid()) {
        DisplayLoadingDialog(); // Checked global.js
        $.ajax({
            url: postUrl,
            type: 'POST',
            data: $('#AddEditAppYearFrm').serialize(),
            success: function (results) {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                if (results.isAdded == true) {
                    $("#SavedNewAppYear").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: 'Configure Appraisal',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                                window.location = "/ConfigurationAppraisal/NewAppraisalYears";
                                $("#AddEditAppYearDialog").dialog("close");
                            }
                        }
                    }); //end dialog
                }
                else if (results.isExisted == true) {
                    $("#appYearExist").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: 'Configure Appraisal',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    }); //end dialog
                }
                else if (results.isAdded == false) {
                    $("#errorAddNewAppYear").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: 'Configure Appraisal',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    }); //end dialog
                }
                else if (results.status == "Error") {
                    $("#errorDialog").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: 'Configure Appraisal',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    }); //end dialog
                }
            },
            error: function (results) {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Configure Appraisal',
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
    else
        return false;
} //end SaveAppraisalNewYear

function EditAppraisalYear(id) {
    var appraisalYearId = id.id;
    var appraisalYearName = id.className;
    $("#loading").dialog(
              {
                  closeOnEscape: false,
                  resizable: false,
                  height: 140,
                  width: 300,
                  modal: true,
                  dialogClass: "noclose"
              });
    $.ajax({
        url: addEditYearUrl,
        type: 'GET',
        data: {
            appraisalYearId: appraisalYearId,
            appraisalYearName: appraisalYearName,
            employeeId: loggedinEmployeeID
        },
        success: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#AddEditAppYearDialog").html(data);
            $("#AddEditAppYearDialog").dialog({
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                width: 500,
                modal: true,
                title: "Add/Edit Appraisal Year",
                close: function (event, ui) {
                    $(this).dialog("close");
                }
            });
        }, //end success
        error: function (result) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#errorDialog").dialog({
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
} //end EditAppraisalYear

function AddAppraisalYear() {
    var appraisalYearId = 0;
    var appraisalYearName = "";
    $("#loading").dialog(
              {
                  closeOnEscape: false,
                  resizable: false,
                  height: 140,
                  width: 300,
                  modal: true,
                  dialogClass: "noclose"
              });
    $.ajax({
        url: addEditYearUrl,
        type: 'GET',
        data: {
            appraisalYearId: appraisalYearId,
            appraisalYearName: appraisalYearName,
            employeeId: loggedinEmployeeID
        },
        success: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#AddEditAppYearDialog").html(data);
            $("#AddEditAppYearDialog").dialog({
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                width: 500,
                modal: true,
                title: "Add/Edit Appraisal Year",
                close: function (event, ui) {
                    $(this).dialog("close");
                }
            });
        }, //end success
        error: function (result) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#errorDialog").dialog({
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
} //end AddAppraisalYear

function DeleteAppraisalYear(id) {
    var appraisalYearId = id.id;
    var appraisalYearName = id.className;
    $("#DeleteAppYearConfirm b").html("");
    $("#DeleteAppYearConfirm b").append(appraisalYearName);
    $("#DeleteAppYearConfirm").dialog({
        closeOnEscape: false,
        resizable: false,
        height: 'auto',
        width: 300,
        modal: true,
        title: "Delete Appraisal Year",
        dialogClass: 'noclose',
        buttons: {
            Ok: function () {
                $("#loading").dialog(
                           {
                               closeOnEscape: false,
                               resizable: false,
                               height: 140,
                               width: 300,
                               modal: true,
                               dialogClass: "noclose"
                           });
                $.ajax({
                    url: deleteYearUrl,
                    data: { appraisalYearId: appraisalYearId },
                    type: 'post',
                    datatype: "json",
                    success: function (result) {
                        $("#loading").dialog("close");
                        $("#loading").dialog("destroy");
                        $("#DeleteAppYearConfirm").dialog("close");
                        if (result.isDeleted == true) {
                            $("#DeleteAppYearSuccess").dialog({
                                closeOnEscape: false,
                                resizable: false,
                                height: 'auto',
                                width: 300,
                                modal: true,
                                title: "Delete Appraisal Year",
                                dialogClass: 'noclose',
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                        window.location = "/ConfigurationAppraisal/NewAppraisalYears";
                                    }
                                }
                            });
                        }
                        else if (result.isExisted == true) {
                            $("#DeleteAppYearExist").dialog({
                                closeOnEscape: false,
                                resizable: false,
                                height: 'auto',
                                width: 300,
                                modal: true,
                                title: "Delete Appraisal Year",
                                dialogClass: 'noclose',
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            });
                        }
                        else if (result.isDeleted == false) {
                            $("#DeleteAppYearError").dialog({
                                closeOnEscape: false,
                                resizable: false,
                                height: 'auto',
                                width: 300,
                                modal: true,
                                title: "Delete Appraisal Year",
                                dialogClass: 'noclose',
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            });
                        }
                        else if (result.status == "Error") {
                            $("#errorDialog").dialog({
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
                    error: function (result) {
                        $("#loading").dialog("close");
                        $("#loading").dialog("destroy");
                        $("#errorDialog").dialog({
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
                }); // ajax End
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
} //end DeleteAppraisalYear