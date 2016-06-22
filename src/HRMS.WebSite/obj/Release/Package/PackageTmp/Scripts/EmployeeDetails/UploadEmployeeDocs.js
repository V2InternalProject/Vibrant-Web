/* File Created: August 14, 2013 */
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
$(document).ready(function () {
    $('#frmUploadEmpDocument #EmployeeId').val(currrentemployeeId);

    if ($('#UserRole').val() != window.HRAdmin || window.employeeStatusId == 2) {
        jQuery('form#frmUploadEmpDocument').find('input,select,textarea').attr('disabled', 'disabled');
        $('#uploadsEmpDoc').hide();
        $('#divUploadDocsDtls').hide();
    }

    $.ajaxSetup({
        // Disable caching of AJAX responses
        cache: false
    });

    $('#empPhoto').change(function () {
        var file = $('input[type="file"]').val();
        var exts = ['exe'];
        // first check if file field has any value
        if (file) {
            $("#EmpfileUploadError").text("");
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array
            if ($.inArray(get_ext[0].toLowerCase(), exts) == 0) {
                $("#EmpUploadError").dialog({
                    title: 'Error',
                    resizable: false,
                    height: 'auto',
                    width: 300,
                    modal: true,
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");

                            if ($.browser.msie) {
                                $('#empPhoto').replaceWith($('#empPhoto').clone(true));
                            } else {
                                $('#empPhoto').val('');
                            }

                            $('#txtFileDescription').val('');
                            $('#txtCurrentState').val('');
                        }
                    },
                    close: function () {
                        if ($.browser.msie) {
                            $('#empPhoto').replaceWith($('#empPhoto').clone(true));
                        } else {
                            $('#empPhoto').val('');
                        }
                        $('#txtFileDescription').val('');
                        $('#txtCurrentState').val('');
                    }
                });
            }
        }
    });

    var employeeid = window.uploadEmployeeid;
    $("#jqTableEmpUploadDoc").jqGrid({
        // Ajax related configurations
        url: window.loadEmployeeUplaodUril,
        //data: { employeeId: employeeid },
        postData: { employeeId: function () { return employeeid; } },
        datatype: "json",
        mtype: "POST",
        // Specify the column names
        colNames: ["Id", "DocumentID", "EmployeeId", "Type", "File Description", "File Name", "FilePath", "Details", ""],

        // Configure the columns
        colModel: [
                { name: "Id", index: "id", width: 40, align: "left", hidden: true },
                { name: "DocumentID", index: "DocumentID", width: 40, align: "left", hidden: true },
                { name: "EmployeeId", index: "EmployeeId", width: 40, align: "left", hidden: true },
                { name: "UploadType", index: "UploadType", width: 35, align: "left", sortable: true },
                { name: "FileDescription", index: "FileDescription", width: 35, align: "left", sortable: true },
                { name: "FileName", index: "FileName", width: 40, align: "left", sortable: true },
                { name: "FilePath", index: "FilePath", width: 35, align: "left", formatter: 'showlink', hidden: true },
                { name: 'Details', index: "Details", width: 20, align: "left", formatter: function () { return '<span class="GridLink">Details</span>'; } },
                {
                    name: "Delete",
                    index: "Delete",
                    width: 20,
                    align: "center",
                    formatter: function () {
                        if ($('#UserRole').val() == window.HRAdmin && (window.employeeStatusId != 2)) {
                            return '<img src="../../Images/New Design/delete-icon.png" width="21px" height="25px">';
                        } else {
                            return '';
                        }
                    }
                }
        ],

        // Grid total width and height
        width: 750,
        // Paging
        toppager: false,
        jsonReader: { repeatitems: false },
        pager: $("#jqTablePagerEmpUploadDoc"),
        rowNum: 20,
        rowList: [],
        viewrecords: true, // Specify if "total number of records" is displayed
        height: 'auto',
        autowidth: false,

        // Grid caption
        //caption: "Upload Document Details",
        gridComplete: function () {
            var grid = $(this).getRowData();
            var count = jQuery("#jqTableEmpUploadDoc").jqGrid('getGridParam', 'records');
            if (count == 0) {
                $('#jqTableEmpUploadDoc').hide();
                $("#UploadEmpContainer ").append("<p>No records found</p>")
            }
            //if (count == 0) {
            //    $('#gbox_jqTableEmpUploadDoc').hide();
            //}
            //else {
            //    $('#gbox_jqTableEmpUploadDoc').show();
            //    if (count > 20) {
            //        $('#jqTablePagerEmpUploadDoc').show();
            //    }
            //    else {
            //        $('#jqTablePagerEmpUploadDoc').hide();
            //    }
            //}
            if (window.employeeStatusId == 2) {
                $("#gbox_jqTableEmpUploadDoc").find('input,select').attr("disabled", true);
                $("#jqTableEmpUploadDoc").jqGrid('hideCol', 'Delete');
            }
        },
        onCellSelect: function (rowid, iCol) {
            var rowData = $(this).getRowData(rowid);
            var selectedDocumentId = rowData['DocumentID'];
            if (iCol == 8) {
                if ($('#UserRole').val() == window.HRAdmin && (window.employeeStatusId != 2)) {
                    DeleteEmpUploadDocsDetail(selectedDocumentId, rowData['EmployeeId']);
                }
            } else {
                if (iCol == 7 && (window.employeeStatusId != 2)) {
                    ShowEmpDocUploadHistoryDetails(selectedDocumentId, rowData['EmployeeId']);
                }
            }
        }
    }).navGrid("#jqTablePagerEmpUploadDoc",
            { search: false, refresh: false, add: false, edit: false, del: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            {} // Search options. Some options can be set on column level
        );

    //    $("#uploadsEmpDoc").click(function () {
    //        if ($("#empPhoto").val() != "") {
    //            if ($('#frmUploadEmpDocument').valid()) {
    //                DisplayLoadingDialog();  //checked
    //            }

    //            $('#frmUploadEmpDocument').ajaxForm({
    //                success: function (results) {
    //                    $("#loading").dialog("close");
    //                    $("#loading").dialog("destroy");
    //                    var st = $.parseJSON(results);

    //                    if (st.status == true) {
    //                        jQuery("#jqTableEmpUploadDoc").trigger("reloadGrid");
    //                        $('#showSucceessUploadDocumentDialog').dialog({
    //                            modal: true,
    //                            resizable: false,
    //                            height: 'auto',
    //                            width: 300,
    //                            title: "Upload Document",
    //                            buttons: {
    //                                "OK": function () {
    //                                    $(this).dialog('close');
    //                                    if ($.browser.msie) {
    //                                        $('#empPhoto').replaceWith($('#empPhoto').clone(true));
    //                                    } else {
    //                                        $('#empPhoto').val('');
    //                                    }
    //                                    $('#txtFileDescription').val('');
    //                                    $('#txtCurrentState').val('');
    //                                    $('#UploadTypeId').val('');
    //                                }
    //                            },
    //                            close: function () {
    //                                if ($.browser.msie) {
    //                                    $('#empPhoto').replaceWith($('#empPhoto').clone(true));
    //                                } else {
    //                                    $('#empPhoto').val('');
    //                                }
    //                                $('#txtFileDescription').val('');
    //                                $('#txtCurrentState').val('');
    //                                $('#UploadTypeId').val('');
    //                            }
    //                        });
    //                    }
    //                    else {
    //                        $("#loading").dialog("close");
    //                        $("#loading").dialog("destroy");
    //                        $('#errorUploadDocumentDialog').dialog({
    //                            modal: true,
    //                            resizable: false,
    //                            height: 'auto',
    //                            width: 300,
    //                            title: "Error",
    //                            buttons: {
    //                                "OK": function () { $(this).dialog('close'); }
    //                            }
    //                        });
    //                    }
    //                },
    //                error: function () {
    //                    $("#loading").dialog("close");
    //                    $("#loading").dialog("destroy");
    //                    $('#errorUploadDocumentDialog').dialog({
    //                        modal: true,
    //                        resizable: false,
    //                        height: 'auto',
    //                        width: 300,
    //                        title: "Error",
    //                        buttons: {
    //                            "OK": function () { $(this).dialog('close'); }
    //                        }
    //                    });
    //                }
    //            });
    //        }
    //        else {
    //            $("#EmpfileUploadError").text("Please select File to upload");
    //            return;
    //        }
    //    });
}); //ready end

function DeleteEmpUploadDocsDetail(selectedDocumentId, empId) {
    $('#deleteAllFileEmpHistoryConfirm').dialog({
        autoOpen: false,
        modal: true,
        width: 300,
        height: 'auto',
        title: "Delete Document",
        dialogClass: 'noclose',
        buttons: {
            "Ok": function () {
                $("#deleteAllFileEmpHistoryConfirm").dialog('close');

                DisplayLoadingDialog(); //checked

                DeletionEmpDocDialog(selectedDocumentId, empId);
            },

            "Cancel": function () { $(this).dialog('close'); }
        }
    });
    $("#deleteAllFileEmpHistoryConfirm").dialog('open');
}

function DeletionEmpDocDialog(selectedDocumentId, empId) {
    $("#DeleteEmpDocUploadSuccessMessage").dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: 300,
        height: 'auto',
        title: "Deleted",
        dialogClass: "noclose",
        buttons: {
            "Ok": function () {
                $("#DeleteEmpDocUploadSuccessMessage").dialog('close');
            }
        }
    });

    $.ajax({
        url: window.DeleteEmployeeUrl,
        data: { documentId: selectedDocumentId, employeeId: empId },
        success: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            if (data == true) {
                jQuery("#jqTableEmpUploadDoc").trigger("reloadGrid");
                $("#deleteAllFileEmpHistoryConfirm").dialog('close');
                $("#DeleteEmpDocUploadSuccessMessage").dialog('open');
            }
            else {
                $("#deleteAllFileEmpHistoryConfirm").dialog('close');
                window.errorOccured();
            }
        },
        Error: function () { window.errorOccured(); }
    });
}

function ShowEmpDocUploadHistoryDetails(selectedDocumentId, empId) {
    $("#ShowEmpDocUploadHistory").dialog({
        autoOpen: false,
        modal: true,
        //resizable: true,
        height: 'auto',
        width: 500,
        title: "Document History",
        dialogClass: "noclose",
        buttons: {
            "Ok": function () {
                $("#ShowEmpDocUploadHistory").dialog('close');
            }
        }
    });

    $.ajax({
        url: window.showHistoryUrl,
        cache: false,
        data: { documentId: selectedDocumentId, employeeId: empId },
        success: function (data) {
            $("#ShowEmpDocUploadHistory").html(data).dialog('open');
            jQuery("#jqTableEmpUploadDoc").trigger("reloadGrid");
        },
        Error: function () { window.errorOccured(); }
    });
}

if ($('#UserRole').val() != window.HRAdmin) {
    $("#frmUploadEmpDocument").find('input,select').attr("disabled", 'disabled');
    $('#uploadsEmpDoc').hide();
    $('#divUploadDocsDtls').hide();
}