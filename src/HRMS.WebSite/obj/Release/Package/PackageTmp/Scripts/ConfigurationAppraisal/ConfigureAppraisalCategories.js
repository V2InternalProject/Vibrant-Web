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
        }
    });
}
function AddAppraisalCategory() {
    var appraisalCategoryId = 0;
    var appraisalCategoryName = "";
    DisplayLoadingDialog(); // checked global.js
    $.ajax({
        url: addEditCategoryUrl,
        type: 'GET',
        data: {
            appraisalCategoryId: appraisalCategoryId,
            appCategoryName: appraisalCategoryName,
            employeeId: loggedinEmployeeID
        },
        success: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#AddEditAppCategoryDialog").html(data);
            $("#AddEditAppCategoryDialog").dialog({
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                width: 500,
                modal: true,
                title: "Add/Edit Appraisal Category",
                close: function (event, ui) {
                    $(this).dialog("close");
                }
            });
        }, //end success
        error: function (result) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            DisplayErrorDialog(); // checked global.js
        }
    });
} //end AddAppraisalCategory

function EditAppraisalCategory(id) {
    var appraisalCategoryId = id.id;
    var appraisalCategoryName = id.className;
    DisplayLoadingDialog(); //checked
    $.ajax({
        url: addEditCategoryUrl,
        type: 'GET',
        data: {
            appraisalCategoryId: appraisalCategoryId,
            appCategoryName: appraisalCategoryName,
            employeeId: loggedinEmployeeID
        },
        success: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#AddEditAppCategoryDialog").html(data);
            $("#AddEditAppCategoryDialog").dialog({
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                width: 500,
                modal: true,
                title: "Add/Edit Appraisal Category",
                close: function (event, ui) {
                    $(this).dialog("close");
                }
            });
        }, //end success
        error: function (result) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            DisplayErrorDialog();
        }
    });
} //end EditAppraisalCategory

function SaveAppraisalNewCategory(postUrl) {
    if ($("#AddEditAppCategoryFrm").valid()) {
        DisplayLoadingDialog(); //checked global.js
        $.ajax({
            url: postUrl,
            type: 'POST',
            data: $('#AddEditAppCategoryFrm').serialize(),
            success: function (results) {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                if (results.isAdded == true) {
                    $("#SavedNewAppCategory").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: 'Configure Appraisal',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                                window.location = "/ConfigurationAppraisal/ConfigureAppraisalCategory";
                                $("#AddEditAppCategoryDialog").dialog("close");
                            }
                        }
                    }); //end dialog
                }
                else if (results.isExisted == true) {
                    $("#appCategoryExist").dialog({
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
                    $("#errorAddNewAppCategory").dialog({
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
                    DisplayErrorDialog();
                }
            },
            error: function () {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                DisplayErrorDialog();
            }
        });
    }
    else
        return false;
} //end SaveAppraisalNewCategory

function DeleteAppraisalCategory(id) {
    var appraisalCategoryId = id.id;
    var appraisalCategoryName = id.className;
    $("#DeleteAppCategoryConfirm b").html("");
    $("#DeleteAppCategoryConfirm b").append(appraisalCategoryName);
    $("#DeleteAppCategoryConfirm").dialog({
        closeOnEscape: false,
        resizable: false,
        height: 'auto',
        width: 300,
        modal: true,
        title: "Delete Appraisal Category",
        dialogClass: 'noclose',
        buttons: {
            Ok: function () {
                DisplayLoadingDialog(); // checked global.js
                $.ajax({
                    url: deleteCategoryUrl,
                    data: { appraisalCategoryId: appraisalCategoryId },
                    type: 'post',
                    datatype: "json",
                    success: function (result) {
                        $("#loading").dialog("close");
                        $("#loading").dialog("destroy");
                        $("#DeleteAppCategoryConfirm").dialog("close");
                        if (result.isDeleted == true) {
                            $("#DeleteAppCategorySuccess").dialog({
                                closeOnEscape: false,
                                resizable: false,
                                height: 'auto',
                                width: 300,
                                modal: true,
                                title: "Delete Appraisal Category",
                                dialogClass: 'noclose',
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                        window.location = "/ConfigurationAppraisal/ConfigureAppraisalCategory";
                                    }
                                }
                            });
                        }
                        else if (result.isDeleted == false) {
                            $("#DeleteAppCategoryError").dialog({
                                closeOnEscape: false,
                                resizable: false,
                                height: 'auto',
                                width: 300,
                                modal: true,
                                title: "Delete Appraisal Category",
                                dialogClass: 'noclose',
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            });
                        }
                        else if (result.isDeleted == "UpdateException") {
                            $("#DeleteAppCategoryUpdateException").dialog({
                                closeOnEscape: false,
                                resizable: false,
                                height: 'auto',
                                width: 300,
                                modal: true,
                                title: "Delete Appraisal Category",
                                dialogClass: 'noclose',
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            });
                        }
                        else if (result.status == "Error") {
                            DisplayErrorDialog("Delete Appraisal Category");
                        }
                    },
                    error: function () {
                        $("#loading").dialog("close");
                        $("#loading").dialog("destroy");
                        DisplayErrorDialog("Delete Appraisal Category");
                    }
                }); // ajax End
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
} //end DeleteAppraisalCategory