$("#btnAddSkillAquiredDetails").click(function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#addSkillAquiredDialog #SkillEmployeeID").val($('#SkillEmployeeID').val());
    $("#addSkillAquiredDialog #SkillsAquiredID").val('');
    $("#addSkillAquiredDialog #SkillName").val('');
    $("#addSkillAquiredDialog #AquiredThrough").val('');
    $("#addSkillAquiredDialog #ProjectUsefulness").val('');
    $("#addSkillAquiredDialog #txtSkillName").val('');
    $("#addSkillAquiredDialog #txtAquiredThrough").val('');
    $("#addSkillAquiredDialog #txtProjectUsefulness").val('');
    $('#addSkillAquiredDialog').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        resizable: false,
        title: "Skill Aquired Details"
    });
    $('#addSkillAquiredDialog').dialog('open');
});

function EditSkillAquiredDetails(Object) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#addSkillAquiredDialog #SkillEmployeeID").val($('#SkillEmployeeID').val());
    $('#addSkillAquiredDialog #SkillsAquiredID').val(Object['SkillsAquiredID']);
    $('#addSkillAquiredDialog #SkillName').val(Object['SkillName']);
    $('#addSkillAquiredDialog #AquiredThrough').val(Object['AquiredThrough']);
    $('#addSkillAquiredDialog #ProjectUsefulness').val(Object['ProjectUsefulness']);
    $('#addSkillAquiredDialog #ProjAchieveID').val(Object['SkillsAquiredID']);
    $('#addSkillAquiredDialog #txtSkillName').val(Object['SkillName']);
    $('#addSkillAquiredDialog #txtAquiredThrough').val(Object['AquiredThrough']);
    $('#addSkillAquiredDialog #txtProjectUsefulness').val(Object['ProjectUsefulness']);
    $('#addSkillAquiredDialog').dialog
        (
            {
                autoOpen: false,
                modal: true,
                width: 500,
                resizable: false,
                title: "Edit Skill Aquired Details"
            }
        );
    $('#addSkillAquiredDialog').dialog('open');
}

function DeleteSkillAquiredDetail(selectedSkillAquiredID) {
    $('#DeleteSkillAquiredDialog').dialog(
            {
                autoOpen: false,
                modal: true,
                width: 300,
                height: 125,
                resizable: false,
                title: "Delete Skill Aquired Details",
                buttons:
                        {
                            "Ok": function () {
                                $.ajax({
                                    url: 'DeleteSkillAquiredDetails',
                                    data: { SkillAquiredID: $('#selectedSkillID').val() },
                                    datatype: "json",
                                    success: function (data) {
                                        if (data.status == true) {
                                            $("#DeleteSkillAquiredDialog").dialog("close");
                                            $("#DeleteSkillAquired").dialog(
                                            {
                                                modal: true,
                                                resizable: false,
                                                height: 140,
                                                width: 300,
                                                title: "Deleted",
                                                buttons:
                                                {
                                                    "Ok": function () {
                                                        $(this).dialog('close');
                                                        jQuery("#SkillAquiredTable").trigger("reloadGrid");
                                                    }
                                                }
                                            }
                                        );
                                        }
                                        else {
                                            $("#errorDialog").dialog({
                                                title: 'Confirmation Process',
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
                                    } //end success
                                });
                            },

                            Cancel: function () {
                                $(this).dialog('close');
                            }
                        }
            }
            );
    $('#DeleteSkillAquiredDialog').dialog('open');
}
/*ADD SKILL AQUIRED */
if (isMangrOrEmp != "Employee") {
    $('#SkillName').attr('disabled', true);
    $('#AquiredThrough').attr('disabled', true);
    $('#ProjectUsefulness').attr('disabled', true);
    $('.btnSkill').hide();
    $('.btnSkillReset').hide();
}
$('#SkillName').rules("add", {
    required: function () {
        return (isMangrOrEmp == "Employee");
    },
    messages:
        {
            required: "Skill Name is required."
        }
});
$('#AquiredThrough').rules("add", {
    required: function () {
        return (isMangrOrEmp == "Employee");
    },
    messages:
        {
            required: "Aquired Through is required."
        }
});

$('#ProjectUsefulness').rules("add", {
    required: function () {
        return (isMangrOrEmp == "Employee");
    },
    messages:
        {
            required: "Project Usefulness is required."
        }
});

var postUrl = 'SaveSkillAquiredInfo/ConfirmationProcess';
$('#saveSkillDetail').off('click').on('click', function () {
    if ($('#addSkillsAquiredDetails').valid()) {
        $.ajax({
            url: postUrl,
            type: 'POST',
            data: $('#addSkillsAquiredDetails').serialize(),
            success: function (results) {
                if (results.status == true) {
                    $('#addSkillAquiredDialog').dialog("close");
                    jQuery("#SkillAquiredTable").trigger("reloadGrid");
                    $("#SkillAquiredSuccessMessege").dialog({
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
                else if (results.status == "Error") {
                    $("#errorDialog").dialog({
                        title: 'Confirmation Process',
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
                    $("#AddSkillAquiredErrorMessege").dialog({
                        title: 'Confirmation Process',
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            }
        });
    }
    return false;
});
/*END HERE*/
var emptyDialogSkillAquired = function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#SkillName").val($('#txtSkillName').val());
    $("#AquiredThrough").val($('#txtAquiredThrough').val());
    $("#ProjectUsefulness").val($('#txtProjectUsefulness').val());
}