$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

//Delete Dependent Details
function DeleteDependantDetail(selectedDependantId, dependentEmployeeId) {
    $('#DeleteConfirmationDialog').dialog(
			{
			    autoOpen: false,
			    modal: true,
			    width: 300,
			    height: 'auto',
			    resizable: false,
			    dialogClass: "noclose",
			    title: "Delete Dependant Detail",
			    buttons:
					{
					    Ok: function () {
					        $.ajax({
					            url: "DeleteDependantDetails/PersonalDetails",
					            data: { DependantID: selectedDependantId, dependentEmployeeId: dependentEmployeeId },
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
					                                jQuery("#dependantTable").trigger("reloadGrid");
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
					                        title: 'Dependent Details',
					                        dialogClass: "noclose",
					                        buttons: {
					                            Ok: function () {
					                                $(this).dialog("close");
					                                jQuery("#dependantTable").trigger("reloadGrid");
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
					                        title: 'Dependent Details',
					                        dialogClass: "noclose",
					                        buttons: {
					                            Ok: function () {
					                                $(this).dialog("close");
					                                jQuery("#dependantTable").trigger("reloadGrid");
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
function EditDependantDetails(Object) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $('#addDependantsDialog #DependandsId').val(Object['DependandsId']);
    $('#addDependantsDialog #DependandsName').val(Object['DependandsName']);
    $("#addDependantsDialog #DependantsDetails_Relation option[value = " + Object['uniqueID'] + "] ").attr('selected', 'selected');
    $('#addDependantsDialog #DependandsBirthDate').val(Object['DependandsBirthDate']);
    $('#addDependantsDialog #DependandsAge').val(Object['DependandsAge']);
    $('#addDependantsDialog #dependentName').val(Object['DependandsName']);
    $('#addDependantsDialog #dependandsRelation').val(Object['uniqueID']);
    $('#addDependantsDialog #birthdateDependant').val(Object['DependandsBirthDate']);
    $('#addDependantsDialog #txtAgeDependant').val(Object['DependandsAge']);
    $('#addDependantsDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Edit Dependant Details"
    }
        );
    $('#addDependantsDialog').dialog('open');
}

$("#btnAddDependantDetails").click(function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#addDependantsDialog #DependandsId").val('');
    $("#addDependantsDialog #DependandsName").val('');
    $("#addDependantsDialog #DependantsDetails_Relation").val('');
    $("#addDependantsDialog #DependandsBirthDate").val('');
    $("#addDependantsDialog #DependandsAge").val('');
    $("#addDependantsDialog #dependentName").val('');
    $("#addDependantsDialog #dependandsRelation").val('');
    $("#addDependantsDialog #birthdateDependant").val('');
    $("#addDependantsDialog #txtAgeDependant").val('');
    $('#addDependantsDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Dependants Details"
    });
    $('#addDependantsDialog').dialog('open');
});

//on change event of date
$("#DependandsBirthDate").change(function () {
    var todate = new Date();
    var fromDate = new Date($('#DependandsBirthDate').val());
    var retToDate = todate.getFullYear() * 12 + todate.getMonth();
    var retFromDate = fromDate.getFullYear() * 12 + fromDate.getMonth();
    var monthDiff = (retToDate - retFromDate) / 12;
    var absMonth = Math.floor(monthDiff);
    var vYrs = absMonth;
    $('#DependandsAge').val(vYrs);
});

//Reset button on dependent
var emptyDialogdependant = function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    if ($('#dependandsRelation').val() == "") {
        $("#uniqueID option[value = " + $('#dependandsRelation').val() + "] ").attr('selected', 'selected');
    }
    else {
        $("#uniqueID option:contains(" + $('#dependandsRelation').val() + ')').attr('selected', 'selected');
    }
    $("#DependantsDetails_Relation option[value = " + $('#dependandsRelation').val() + "] ").attr('selected', 'selected');
    $("#DependandsName").val($('#dependentName').val());
    $("#DependandsBirthDate").val($('#birthdateDependant').val());
    $("#DependandsAge").val($("#addDependantsDetails #txtAgeDependant").val());
}

function SaveDependentDetails() {
    if ($('#addDependantsDetails').valid()) {
        $.ajax({
            url: "SaveDependantInfo/PersonalDetails",
            type: 'POST',
            data: $('#addDependantsDetails').serialize(),
            success: function (results) {
                if (results.status == true) {
                    $('#addDependantsDialog').dialog("close");
                    jQuery("#dependantTable").trigger("reloadGrid");
                    $("#AddDependantSuccessMessege").dialog({
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
                    $('#addDependantsDialog').dialog("close");
                    $("#errorDialog").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: 'Dependent Details',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                                jQuery("#dependantTable").trigger("reloadGrid");
                            }
                        }
                    }); //end dialog
                }
                else {
                    $('#addDependantsDialog').dialog("close");
                    $("#AddDependantErrorMessege").dialog({
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                                jQuery("#dependantTable").trigger("reloadGrid");
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
}//end function

function ChangeDependentRelation(e) {
    var relationName = e.target[e.target.selectedIndex].text;
    var ID = 0;
    $.each(stringRelationList, function (index, value) {
        if (value.DependandsRelation == relationName) {
            ID = value.uniqueID;
        }
    });
    $('#DependantsDetailsForm #uniqueID').val(ID);
    $('#' + SelectedDependentRowId + '_DependandsRelation').attr('title', relationName);
}//end function

function ChangeDependandsBirthDate(e) {
    var selectedDateID = e.target.id;
    var rowID = selectedDateID.split("_");

    var todate = new Date();
    var fromDate = new Date(e.target.value);
    var retToDate = todate.getFullYear() * 12 + todate.getMonth();
    var retFromDate = fromDate.getFullYear() * 12 + fromDate.getMonth();
    var monthDiff = (retToDate - retFromDate) / 12;
    var absMonth = Math.floor(monthDiff);
    var vYrs = absMonth;
    if (rowID[0] == "new")
        $("#" + rowID[0] + "_" + rowID[1] + "_DependandsAge").val(vYrs);
    else
        $("#" + rowID[0] + "_DependandsAge").val(vYrs);
}//end function