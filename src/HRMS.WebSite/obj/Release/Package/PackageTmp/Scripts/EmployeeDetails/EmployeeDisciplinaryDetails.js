/* File Created: August 13, 2013 */

function AddDisciplinaryDetails() {
    $('#AddDiscipline').dialog({
        autoOpen: false,
        modal: true,
        width: '500',
        resizable: false,
        title: 'Discipline Details'
    });

    $('#AddDiscipline #hdnAddDisciplineDetailsDisciplineId').val(0);
    $('#AddDiscipline #AddedDate').val('');
    $('#AddDiscipline #DisciplineSubject').val('');
    $('#AddDiscipline #DisciplineMessage').val('');
    $('#AddDiscipline #Manager').val('');
    $('#AddDiscipline #addedDate').val('');
    $('#AddDiscipline #disciplineSubject').val('');
    $('#AddDiscipline #disciplineMessage').val('');
    $('#AddDiscipline #manager').val('');
    $('#AddDiscipline').dialog('open');
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
}

function EditEmployeeDisciplinaryDetails(object) {
    if ((object['CreatedByUserId']) == window.CurrentUserName) {
        $('#AddDiscipline').dialog({
            autoOpen: false,
            modal: true,
            width: '500',
            resizable: false,
            title: 'Discipline Details'
        });
        $(".field-validation-error").empty();
        $('input').removeClass("input-validation-error");
        $('#AddDiscipline #hdnAddDisciplineDetailsDisciplineId').val(object['DisciplineId']);
        $('#AddDiscipline #AddedDate').val(object['AddedDate']);
        $('#AddDiscipline #DisciplineSubject').val(object['DisciplineSubject']);
        $('#AddDiscipline #DisciplineMessage').val(object['DisciplineMessage']);
        $('#AddDiscipline #Manager').val(object['ManagerId']);
        $('#AddDiscipline #hdnAddDisciplineDetailsManagerId').val(object['ManagerId']);
        $('#AddDiscipline #hdnAddDisciplineDetailsEmployeeId').val(window.currentemployeeId);
        $('#AddDiscipline #addedDate').val(object['AddedDate']);
        $('#AddDiscipline #disciplineSubject').val(object['DisciplineSubject']);
        $('#AddDiscipline #disciplineMessage').val(object['DisciplineMessage']);
        $('#AddDiscipline #manager').val(object['Manager']);

        $('#AddDiscipline').dialog('open');
    }
}

function ShowDisciplinaryDetails(object) {
    $('#txtAddedDate').val(object['AddedDate']);
    $('#txtDisplayDisciplineSubject').val(object['DisciplineSubject']);
    $('#txtDisplayDisciplineMessage').val(object['DisciplineMessage']);
    $('#lbltxtAddedDate').text(object['AddedDate']);
    $('#lbltxtDisplayDisciplineSubject').text(object['DisciplineSubject']);
    $('#lbltxtDisplayDisciplineMessage').text(object['DisciplineMessage']);

    $('#DisplayDiscipline').dialog({
        modal: true,
        width: '450',
        resizable: false,
        dialogClass: "noclose",
        title: 'Discipline Details',
        buttons: {
            "Ok": function () {
                //$("#DisplayDiscipline").dialog('close');
                $(this).dialog('close');
            }
        }
    });
}

function DeleteQualificationDetail(selectedQualId, object) {
    if ((object['CreatedByUserId']) == window.CurrentUserName) {
        $('#deleteRecordConfirmation').dialog({
            autoOpen: false,
            modal: true,
            resizable: false,
            height: 'auto',
            width: 300,
            dialogClass: "noclose",
            title: 'Delete Discipline Details',
            buttons: {
                "Ok": function () { DeletionDialog(selectedQualId); },
                "Cancel": function () { $(this).dialog('close'); }
            }
        });
        $("#deleteRecordConfirmation").dialog('open');
    }
}

function DeletionDialog(selectedDisciplineId) {
    $("#DeleteRecord").dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        height: 140,
        width: 300,
        title: "Deleted",
        dialogClass: "noclose",
        buttons: {
            "Ok": function () {
                $(this).dialog('close');
            }
        }
    });

    $.ajax({
        url: window.DeleteDisciplineDetailsUrlAction,
        data: { disciplineId: selectedDisciplineId },
        success: function (data) {
            jQuery("#disciplinedetailsjqTable").trigger("reloadGrid");
            $("#deleteRecordConfirmation").dialog('close');
            $("#DeleteRecord").dialog('open');
        },
        Error: function () { window.errorOccured(); }
    });
}

function refreshGridView() {
    jQuery("#disciplinedetailsjqTable").trigger("reloadGrid");
}