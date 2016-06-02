$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

//Delete Dependent Details
function DeleteDecalrationDetail(selecteddeclarationId, declarationEmployeeId) {
    $('#DeleteConfirmationDialog').dialog(
			{
			    autoOpen: false,
			    modal: true,
			    width: 300,
			    height: 'auto',
			    resizable: false,
			    dialogClass: "noclose",
			    title: "Delete Declaration Detail",
			    buttons:
					{
					    Ok: function () {
					        $.ajax({
					            url: "DeleteDeclarationDetails/PersonalDetails",
					            data: { DeclarationID: selecteddeclarationId, DeclarationEmployeeId: declarationEmployeeId },
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
					                                $(this).dialog('close');
					                                jQuery("#declarationTable").trigger("reloadGrid");
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
					                        title: 'Declaration Details',
					                        dialogClass: "noclose",
					                        buttons: {
					                            Ok: function () {
					                                $(this).dialog("close");
					                                jQuery("#declarationTable").trigger("reloadGrid");
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
					                        title: 'Declaration Details',
					                        dialogClass: "noclose",
					                        buttons: {
					                            Ok: function () {
					                                $(this).dialog("close");
					                                jQuery("#declarationTable").trigger("reloadGrid");
					                            }
					                        }
					                    }); //end dialog
					                }
					            }
					        });
					    },
					    Cancel: function () {
					        $(this).dialog('close');
					    }
					}
			}
			);
    $('#DeleteConfirmationDialog').dialog('open');
}

//Edit dependent details
function EditDeclarationDetails(Object) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $('#addDeclarationDialog #DeclarationId').val(Object['DeclarationId']);
    $('#addDeclarationDialog #Name').val(Object['Name']);
    $('#addDeclarationDialog #declarationName').val(Object['Name']);
    $("#addDeclarationDialog #DeclarationDetails_Relation option[value = " + Object['uniqueID'] + "] ").attr('selected', 'selected');
    $("#addDeclarationDialog #DeclarationV2Employee_Name option[value = " + Object['V2EmployeeID'] + "] ").attr('selected', 'selected');
    $('#addDeclarationDialog #declarationRelation').val(Object['uniqueID']);
    $('#addDeclarationDialog #declarationV2EmpName').val(Object['V2EmployeeID']);
    $('#addDeclarationDialog #EmployeeCode').val(Object['EmployeeCode']);
    $('#addDeclarationDialog #declarationEmployeeCode').val(Object['EmployeeCode']);

    $('#addDeclarationDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Edit Declaration Details"
    }
        );
    $('#addDeclarationDialog').dialog('open');
}

$("#btnAddDeclarationDetails").click(function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#addDeclarationDialog #DeclarationId").val('');
    $("#addDeclarationDialog #Name").val('');
    $("#addDeclarationDialog #declarationName").val('');
    $("#addDeclarationDialog #DeclarationDetails_Relation").val('');
    $("#addDeclarationDialog #declarationRelation").val('');
    $("#addDeclarationDialog #DeclarationV2Employee_Name").val('');
    $("#addDeclarationDialog #declarationV2EmpName").val('');
    $("#addDeclarationDialog #EmployeeCode").val('');
    $("#addDeclarationDialog #declarationEmployeeCode").val('');
    $('#addDeclarationDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Declaration Details"
    });
    $('#addDeclarationDialog').dialog('open');
});

//Reset button on dependent
var emptyDialogdeclaration = function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");

    $("#Name").val($('#declarationName').val());

    if ($('#declarationRelation').val() == "") {
        $("#uniqueID option[value = " + $('#declarationRelation').val() + "] ").attr('selected', 'selected');
    }
    else {
        $("#uniqueID option:contains(" + $('#declarationRelation').val() + ')').attr('selected', 'selected');
    }
    $("#DeclarationDetails_Relation option[value = " + $('#declarationRelation').val() + "] ").attr('selected', 'selected');

    if ($('#declarationV2EmpName').val() == "") {
        $("#V2EmployeeID option[value = " + $('#declarationV2EmpName').val() + "] ").attr('selected', 'selected');
    }
    else {
        $("#V2EmployeeID option:contains(" + $('#declarationV2EmpName').val() + ')').attr('selected', 'selected');
    }
    $("#DeclarationV2Employee_Name option[value = " + $('#declarationV2EmpName').val() + "] ").attr('selected', 'selected');

    $("#EmployeeCode").val($('#declarationEmployeeCode').val());
    //

    //    $("#DependandsAge").val($("#addDeclarationsDetails #txtAgeDependant").val());
}

function SaveDeclarationDetails() {
    if ($('#addDeclarationsDetails').valid()) {
        $.ajax({
            url: "SaveDecalrationInfo/PersonalDetails",
            type: 'POST',
            data: $('#addDeclarationsDetails').serialize(),
            success: function (results) {
                if (results.status == true) {
                    $('#addDeclarationDialog').dialog("close");
                    jQuery("#declarationTable").trigger("reloadGrid");
                    $("#AddDeclarationSuccessMessege").dialog({
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
                else if (results.status == "Error") {
                    $('#addDeclarationDialog').dialog("close");
                    $("#errorDialog").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: 'Declaration Details',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                                jQuery("#declarationTable").trigger("reloadGrid");
                            }
                        }
                    }); //end dialog
                }
                else {
                    $('#addDeclarationDialog').dialog("close");
                    $("#AddDeclarationErrorMessege").dialog({
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                                jQuery("#declarationTable").trigger("reloadGrid");
                            }
                        }
                    });
                }
            }
        });
        return true;
    }
    else
        return false;
}

function isRelationSelected(value, colname) {
    if (value == "0") {
        $("#RelationRequiredDialog").dialog({
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
} //end function

function isStatusSelected(value, colname) {
    if (value == "0") {
        $("#StatusRequiredDialog").dialog({
            modal: true,
            resizable: false,
            height: 'auto',
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
} //end function

function ChangeDeclarationRelation(e) {
    var relationName = e.target[e.target.selectedIndex].text;
    var ID = 0;
    $.each(stringRelationList, function (index, value) {
        if (value.DependandsRelation == relationName) {
            ID = value.uniqueID;
        }
    });
    $('#DeclarationDetailsForm #uniqueID').val(ID);
    $('#' + SelectedDeclarationRowId + '_RelationshipName').attr('title', relationName);
} //end function

function ChangeStatus(e) {
    var statusName = e.target[e.target.selectedIndex].text;
    var ID = 0;
    $.each(stringStatusList, function (index, value) {
        if (value.V2EmployeeName == statusName) {
            ID = value.V2EmployeeID;
        }
    });
    $('#DeclarationDetailsForm #V2EmployeeID').val(ID);
    $('#' + SelectedDeclarationRowId + '_V2EmployeeName').attr('title', statusName);
} //end function

function isValidEmployeeCode(value, Colname) {
    var pattern = new RegExp(/^\+?[0-9]+$/);
    var valid = pattern.test(value);
    if (!valid) {
        $("#ValidEmployeeCodeDialog").dialog({
            modal: true,
            resizable: false,
            height: 'auto',
            width: 300,
            dialogClass: "noclose",
            buttons: {
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
} //end function