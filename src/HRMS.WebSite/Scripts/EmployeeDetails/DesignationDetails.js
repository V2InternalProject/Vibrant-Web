/* File Created: August 13, 2013 */

var AddDesignationDetails = function () {
    $('#newdesignationdialog').dialog({
        autoOpen: false,
        modal: true,
        width: '500',
        resizable: false,
        height: 'auto',
        title: 'Designation Details'
    });

    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");

    $('#newdesignationdialog #Year').val('');
    $('#newdesignationdialog #Month').val('');
    $('#newdesignationdialog #Grade').val('');
    $('#newdesignationdialog #Level').val('');
    $('#newdesignationdialog #NewDesignation').val('');
    $('#newdesignationdialog #RoleDescription').val('');
    $('#newdesignationdialog #JoiningDesignation').val('');
    $('#newdesignationdialog #UniqueId').val('');

    $('#newdesignationdialog #year').val('');
    $('#newdesignationdialog #month').val('');
    $('#newdesignationdialog #grade').val('');
    $('#newdesignationdialog #level').val('');
    $('#newdesignationdialog #designation').val('');
    $('#newdesignationdialog #roleDescription').val('');
    $('#newdesignationdialog #joiningDesignation').val('');

    $('#newdesignationdialog').dialog('open');
};

var EditDesignationDetails = function (object) {
    $('#newdesignationdialog').dialog({
        autoOpen: false,
        modal: true,
        width: '500',
        resizable: false,
        height: 'auto',
        title: 'Designation Details'
    });

    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");

    $('#newdesignationdialog #Year').val(object['Year']);
    $('#newdesignationdialog #Month').val(object['Month']);
    //    $('#newdesignationdialog #Grade option:contains(' + object['GradeId'] + ')').attr("selected", "selected");
    $('#newdesignationdialog #Grade').val(object['GradeId']);
    $('#newdesignationdialog #Level').val(object['Level']);
    $('#newdesignationdialog #NewDesignation').val(object['Designation']);
    $('#newdesignationdialog #RoleDescription').val(object['RoleDescription']);
    $('#newdesignationdialog #JoiningDesignation').val(object['JoiningDesignation']);
    $('#newdesignationdialog #UniqueId').val(object['UniqueId']);
    $('#newdesignationdialog #year').val(object['Year']);
    $('#newdesignationdialog #month').val(object['Month']);
    $('#newdesignationdialog #grade').val(object['GradeId']);
    $('#newdesignationdialog #level').val(object['Level']);
    $('#newdesignationdialog #designation').val(object['Designation']);
    $('#newdesignationdialog #roleDescription').val(object['RoleDescription']);
    $('#newdesignationdialog #joiningDesignation').val(object['JoiningDesignation']);

    $('#newdesignationdialog').dialog('open');
};

var DeleteDesignationDetail = function (selectedDesignationId, isDefaultRecord, employeeId) {
    if (isDefaultRecord == true || isDefaultRecord == 'true') {
        $('#deleteRecordConfirmation p').html("");
        $('#deleteRecordConfirmation p').append("This is Default record are you sure you want to delete this default record?");
    }
    else {
        $('#deleteRecordConfirmation p').html("");
        $('#deleteRecordConfirmation p').append("Are you sure you want to delete this record?");
    }
    $('#deleteRecordConfirmation').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        height: 'auto',
        width: 300,
        title: "Delete Designation Detail",
        dialogClass: "noclose",
        buttons:
        {
            "Ok": function () {
                $.ajax({
                    url: window.deleteRecordConfirmationUrl,
                    data: {
                        designationId: selectedDesignationId,
                        isDefaultRecord: isDefaultRecord,
                        employeeId: employeeId
                    },
                    success: function (data) {
                        $("#deleteRecordConfirmation").dialog("close");
                        $("#deleteRecordConfirmation").dialog("destroy");
                        $("#DeleteRecord").dialog({
                            modal: true,
                            resizable: false,
                            height: 140,
                            width: 300,
                            title: "Deleted",
                            dialogClass: "noclose",
                            buttons:
                            {
                                "Ok": function () {
                                    jQuery("#jqDesignationTable").trigger("reloadGrid");
                                    $(this).dialog('close');
                                }
                            }
                        });
                    }
                });
            },
            "Cancel": function () {
                $(this).dialog('close');
            }
        }
    });
    $('#deleteRecordConfirmation').dialog('open');
};