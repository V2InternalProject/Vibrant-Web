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
 function AddExpProject() {
    var expProjectId = 0;
    var expProjectName = "";
    DisplayLoadingDialog();  //checked
    $.ajax({
        url: addEditProjectUrl,
        type: 'GET',
        data: { expProjectId: expProjectId,
            expProjectName: expProjectName,
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
                title: "Add/Edit Expense Reimbursement Project",
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
} //end AddExpProject

function EditExpProject(id) {
    var expProjectId = id.id;
    var expProjectName = id.className;
    DisplayLoadingDialog(); //checked
    $.ajax({
        url: addEditProjectUrl,
        type: 'GET',
        data: { expProjectId: expProjectId,
            expProjectName: expProjectName,
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
                title: "Add/Edit Expense Reimbursement Project",
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
} //end EditExpProject

function SaveExpNewProject(postUrl) {
    if ($("#AddEditExpProjectFrm").valid()) {
        DisplayLoadingDialog();  //checked
        $.ajax({
            url: postUrl,
            type: 'POST',
            data: $('#AddEditExpProjectFrm').serialize(),
            success: function (results) {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                if (results.isAdded == true) {
                    $("#SavedNewAppCategory").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: 'Configure Expense Reimbursement',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                                window.location = "/ExpenseReimbursement/ConfigureExpenseReimbProjectNames";
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
                        title: 'Configure Expense Reimbursement',
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
                        title: 'Configure Expense Reimbursement',
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
} //end SaveExpNewProject

function DeleteExpProject(id) {
    var expProjectId = id.id;
    var expProjectName = id.className;
    $("#DeleteAppCategoryConfirm b").html("");
    $("#DeleteAppCategoryConfirm b").append(expProjectName);
    $("#DeleteAppCategoryConfirm").dialog({
        closeOnEscape: false,
        resizable: false,
        height: 'auto',
        width: 300,
        modal: true,
        title: "Delete Expense Reimbursement Project",
        dialogClass: 'noclose',
        buttons: {
            Ok: function () {
                DisplayLoadingDialog();  //checked
                $.ajax({
                    url: deleteProjectUrl,
                    data: { expProjectId: expProjectId },
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
                                title: "Delete Expense Reimbursement Project",
                                dialogClass: 'noclose',
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                        window.location = "/ExpenseReimbursement/ConfigureExpenseReimbProjectNames";
                                    }
                                }
                            });
                        }
                        else if (result.isDeleted == false || result.isDeleted == "UpdateException") {
                            $("#DeleteAppCategoryError").dialog({
                                closeOnEscape: false,
                                resizable: false,
                                height: 'auto',
                                width: 300,
                                modal: true,
                                title: "Delete Expense Reimbursement Project",
                                dialogClass: 'noclose',
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            });
                        }
                        else if (result.status == "Error") {
                            DisplayErrorDialog();
                        }
                    },
                    error: function () {
                        $("#loading").dialog("close");
                        $("#loading").dialog("destroy");
                        DisplayErrorDialog();
                    }
                }); // ajax End
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
} //end DeleteExpProject