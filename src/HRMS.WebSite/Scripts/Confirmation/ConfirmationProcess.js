$(document).ready(function () {
    $("#inbox").click(function () {
        $("#gbox_ConfirmationTableInbox").show();
        $("#gbox_ConfirmationTableWatchList").hide();
    });

    $("#watchlist").click(function () {
        $("#gbox_ConfirmationTableInbox").hide();
        $("#gbox_ConfirmationTableWatchList").show();
    });

    $("#all").click(function () {
        $("#gbox_ConfirmationTableInbox").show();
        $("#gbox_ConfirmationTableWatchList").show();
    });

    $("#btnshow").click(function () {
        jQuery("#ConfirmationTableInbox").trigger("reloadGrid");
        jQuery("#ConfirmationTableWatchList").trigger("reloadGrid");
    });

    $("#btnShowStatus").click(function () {
        jQuery("#ConfirmationShowStatusTable").trigger("reloadGrid");
        $('#ShowStatusDialog').dialog({
            autoOpen: true,
            modal: true,
            width: 1000,
            resizable: true,
            title: "Show Status"
        });
    });

    $("#btnEmployeeForm").click(function () {
        var encyptedEmplId = $("#hiddenid").val();
        window.location.href = 'ConfirmationDetails/ConfirmationProcess?employeeId=' + encyptedEmplId;
    });
    $("#btnShowDetails").click(function () {
        var encyptedEmplId = $("#hiddenid").val();
        window.location.href = 'ConfirmationDetails/ConfirmationProcess?employeeId=' + encyptedEmplId + '&viewDetailsBtn=yes';
    });
    $("#btnManagerComments").click(function () {
        var encyptedEmplId = $("#hiddenid").val();
        window.location.href = 'ConfirmationDetails/ConfirmationProcess?employeeId=' + encyptedEmplId;
    });

    $("#btnReviewerComments").click(function () {
        var encyptedEmplId = $("#hiddenid").val();
        window.location.href = 'ConfirmationDetails/ConfirmationProcess?employeeId=' + encyptedEmplId;
    });
    $("#btnHRComments").click(function () {
        var encyptedEmplId = $("#hiddenid").val();
        window.location.href = 'ConfirmationDetails/ConfirmationProcess?employeeId=' + encyptedEmplId;
    });

    $("#txtSearchTextConfrmPrc").autocomplete({
        source: function (request, response) {
            $.getJSON('SearchEmployeeAutoSuggestCnfrm/ConfirmationProcess', { term: request.term }, function (data) {
                response($.map(data, function (el) {
                    var emplyeeInformation = el.EmployeeName + " {" + el.EmployeeCode + "}";
                    return {
                        label: emplyeeInformation,
                        value: el.EmployeeName
                    };
                }));
            });
        }
    });

    function EncryptedId() {
        var id = "";
        var posturl = 'EmployeeEncryption/ConfirmationProcess';
        var Parameter = { employeeId: $("#hiddenid").val() };
        $.ajax(
             {
                 url: posturl,
                 type: 'POST',
                 async: false,
                 data: Parameter,
                 dataType: 'json',
                 success: function (data) {
                     id = data.result;
                 },
                 error: function () {
                     return false;
                 }
             });   //end ajax
        return id;
    }
    var searchterm = $("#txtSearchTextConfrmPrc").val();
    SetPageTitle('Initiate Confirmation Process');
    var deletionIdSelected = 0;
    $("#ConfirmationTableInbox").jqGrid({
        // Ajax related configurations
        url: 'InboxConfirmationProcessLoadGrid/ConfirmationProcess',
        datatype: "json",
        mtype: "POST",
        postData: { term: function () { return $("#txtSearchTextConfrmPrc").val(); }, Field: function () { return $("#Field").val(); }, FieldChild: function () { return $("#FieldChild").val(); } },
        // Specify the column names
        colNames: ["Confirmation ID", "Employee Code", "Employee Id", "Encrypted Employee Id", "Employee Name", "Status", "Joining Date", "Probation Review Date", "Initiated Date", "Stage ID", "Stage", "Stage ActorID", "Stage ActorName"],
        // Configure the columns
        colModel: [
            { name: "ConfirmationID", index: "ConfirmationID", hidden: true, width: 50, align: "left" },
                { name: "EmployeeCode", index: "EmployeeCode", hidden: true, width: 50, align: "left" },
                { name: "EmployeeId", index: "EmployeeId", hidden: true, width: 50, align: "left" },
                { name: "encryptedEmployeeId", index: "encryptedEmployeeId", hidden: true, width: 50, align: "left" },
                { name: "EmployeeName", index: "EmployeeName", width: 50, align: "left", formatter: formatlink },
                { name: "Status", index: "Status", width: 100, align: "left", formatter: RenderImages },
                { name: "JoiningDate", index: "JoiningDate", width: 50, align: "left", sorttype: 'date', formatter: 'date' },
                { name: "ProbationReviewDate", index: "ProbationReviewDate", width: 50, align: "left", sorttype: 'date', formatter: 'date' },
                { name: "InitiatedDate", index: "InitiatedDate", width: 50, align: "left", sorttype: 'date', formatter: 'date' },
                { name: "StageID", index: "StageID", hidden: true, width: 50, align: "left" },
                { name: "Stage", index: "Stage", width: 50, align: "left" },
                { name: "StageActorID", index: "StageActorID", hidden: true, width: 50, align: "left" },
                { name: "StageActorName", index: "StageActorName", hidden: true, width: 50, align: "left" }

            ],
        width: 700,
        height: 200,
        // Paging
        toppager: false,
        jsonReader: { repeatitems: false },
        pager: $("#ConfirmationTablePagerInbox"),
        rowNum: 5,
        rowList: [5, 10, 20], viewrecords: true, // Specify if "total number of records" is displayed
        height: 'auto',
        autowidth: false,
        //caption: "Inbox",
        onCellSelect: function (rowid, iCol) {
            var rowData = $(this).getRowData(rowid);
            var selectedDependantId = rowData['EmployeeId']
        }
    }).navGrid("#ConfirmationTablePagerInbox",
                                            { search: false, refresh: false, add: false, edit: false, del: false }
                                           );

    $("#ConfirmationTableWatchList").jqGrid({
        // Ajax related configurations
        url: 'WatchListConfirmationProcessLoadGrid/ConfirmationProcess',
        datatype: "json",
        mtype: "POST",
        postData: { term: function () { return $("#txtSearchTextConfrmPrc").val(); }, Field: function () { return $("#Field").val(); }, FieldChild: function () { return $("#FieldChild").val(); } },
        // Specify the column names
        colNames: ["Confirmation ID", "Employee Code", "Employee Id", "Encrypted Employee Id", "Employee Name", "Status", "Joining Date", "Probation Review Date", "Initiated Date", "Stage ID", "Stage", "Stage ActorID", "Stage ActorName", "ActionType"],
        // Configure the columns
        colModel: [
            { name: "ConfirmationID", index: "ConfirmationID", hidden: true, width: 50, align: "left" },
                { name: "EmployeeCode", index: "EmployeeCode", hidden: true, width: 50, align: "left" },
                { name: "EmployeeId", index: "EmployeeId", hidden: true, width: 50, align: "left" },
                 { name: "encryptedEmployeeId", index: "encryptedEmployeeId", hidden: true, width: 50, align: "left" },
                { name: "EmployeeName", index: "EmployeeName", width: 50, align: "left", formatter: formatlink },
                { name: "Status", index: "Status", width: 100, formatter: RenderImages, align: "left" },
                { name: "JoiningDate", index: "JoiningDate", width: 50, align: "left", sorttype: 'date', formatter: 'date' },
                { name: "ProbationReviewDate", index: "ProbationReviewDate", width: 50, align: "left", sorttype: 'date', formatter: 'date' },
                { name: "InitiatedDate", index: "InitiatedDate", width: 50, align: "left", sorttype: 'date', formatter: 'date' },
                { name: "StageID", index: "StageID", hidden: true, width: 50, align: "left" },
                { name: "Stage", index: "Stage", width: 50, align: "left" },
                 { name: "StageActorID", index: "StageActorID", hidden: true, width: 50, align: "left" },
                { name: "StageActorName", index: "StageActorName", hidden: true, width: 50, align: "left" },
                 { name: "Field", index: "Field", hidden: true, width: 50, align: "left" }
            ],
        width: 700,
        height: 200,
        // Paging
        toppager: false,
        jsonReader: { repeatitems: false },
        pager: $("#ConfirmationTablePagerWatchList"),
        rowNum: 5,
        rowList: [5, 10, 20],
        viewrecords: true, // Specify if "total number of records" is displayed
        height: 'auto',
        autowidth: false,
        //caption: "WatchList",
        onCellSelect: function (rowid, iCol, rowobject) {
            var rowData = $(this).getRowData(rowid);
            var selectedDependantId = rowData['EmployeeId'];
        }
    }).navGrid("#ConfirmationTablePagerWatchList",
                   { search: false, refresh: false, add: false, edit: false, del: false }
                );
    $("#gbox_ConfirmationTableWatchList").hide();
    $("#Field").change(function () {
        if ($("#Field").val() != "") {
            var url = 'GetFieldDropdownDetails/ConfirmationProcess';
            $("#FieldChild").show();
            //fire off the request, passing it the MatserId which is the employeementStatus selected item value
            $.getJSON(url, { FieldName: $("#Field").val() }, function (data) {
                //Clear the Model list
                $("#FieldChild").empty();
                $("#FieldChild").append("<option value='" + "" + "'>" + "Select" + "</option>");
                //Foreach Model in the list, add a model option from the data returned
                $.each(data, function (index, optionData) {
                    $("#FieldChild").append("<option value='" + optionData.ID + "'>" + optionData.Discription + "</option>");
                });
            });
        }
        else {
            $("#FieldChild").val("");
            $("#FieldChild").hide();
        }
    });
});

function formatlink(cellvalue, options, rowobject) {
    var status = rowobject['Field'];
    var encryptedEmployeeId = rowobject['encryptedEmployeeId'];
    if (rowobject['Field'] == 'Move Ahead')
        status = 'moveahead';
    return "<a href=# id=" + rowobject['EmployeeId'] + " class=EmpLink onClick = LinkClick(this," + rowobject['StageID'] + ",'" + encryptedEmployeeId + "'" + ",'" + status + "'" + ",'" + rowobject['ConfirmationID'] + "') class=EmployeeNameLink >" + cellvalue + "</a>";
}

function RenderImages(cellvalue, options, rowobject) {
    var obj;
    obj = "";
    for (var i = 1; i < 8; i++) {
        if (rowobject['StageID'] >= i) {
            if (rowobject['Field'] == "Reject" && rowobject['StageID'] == i) {
                obj = obj + "<img src='../../Images/New Design/status-rejected.png' width='31px' height='31px' class='StatusImagesRed'> "; //  Red
            } else if (rowobject['StageID'] != i) {
                obj = obj + "<img src='../../Images/New Design/status-on.png' width='31px' height='31px' class='StatusImagesGreen'> "; // Green
            }
            else {
                if (rowobject['StageID'] == 7) {
                    obj = obj + "<img src='../../Images/New Design/status-on.png' width='31px' height='31px' class='StatusImagesGreen'> "; // Green
                }
                else {
                    obj = obj + "<img src='../../Images/New Design/status-off.png' width='31px' height='31px' class='StatusImagesGray'>"; //  Yellow
                }
            }
        } else {
            obj = obj + "<img src='../../Images/New Design/status-off.png' width='31px' height='31px' class='StatusImagesGray'>"; //  Yellow
        }
    }
    return obj;
}
function LinkClick(event, StageId, EncryptedId, ActionType, ConfirmationID) {
    $("#hdnConfirmationId").val(ConfirmationID);
    var reject = ActionType;
    var target = event.id;
    var width = $("#" + target + "").width();
    var positionlink = $("#" + target + "").offset();
    var table = $("#" + target + "").parent().parent().parent().parent();
    var tableid = $(table).attr('id');
    if (tableid == "ConfirmationTableInbox") {
        if (StageId == 1) {
            $("#btnEmployeeForm").hide();
            $("#btnReviewerComments").hide();
            $("#btnManagerComments").hide();
            $("#btnHRComments").hide();
            $("#btnShowDetails").show();
        }
        else if (StageId == 3) {
            $("#btnEmployeeForm").show();
            $("#btnReviewerComments").hide();
            $("#btnManagerComments").hide();
            $("#btnHRComments").hide();
            $("#btnShowDetails").hide();
        }
        else if (StageId == 4) {
            $("#btnEmployeeForm").hide();
            $("#btnReviewerComments").hide();
            $("#btnManagerComments").show();
            $("#btnHRComments").hide();
        }
        else if (StageId == 5) {
            $("#btnEmployeeForm").hide();
            $("#btnReviewerComments").show();
            $("#btnManagerComments").hide();
            $("#btnHRComments").hide();
        }
        else if (StageId == 6) {
            $("#btnEmployeeForm").hide();
            $("#btnReviewerComments").hide();
            $("#btnManagerComments").hide();
            $("#btnHRComments").show();
        }
        else {
            $("#btnEmployeeForm").hide();
            $("#btnReviewerComments").hide();
            $("#btnManagerComments").hide();
            $("#btnHRComments").hide();
            $("#btnShowDetails").show();
        }
    }
    if (tableid == "ConfirmationTableWatchList") {
        if (StageId == 1) {
            $("#btnEmployeeForm").hide();
            $("#btnReviewerComments").hide();
            $("#btnManagerComments").hide();
            $("#btnHRComments").hide();
            $("#btnShowDetails").show();
        }
        else if (StageId == 3) {
            $("#btnEmployeeForm").hide();
            $("#btnReviewerComments").hide();
            $("#btnManagerComments").hide();
            $("#btnHRComments").hide();
            if (reject == 'Reject')
                $("#btnShowDetails").show();
        }
        else {
            $("#btnEmployeeForm").hide();
            $("#btnReviewerComments").hide();
            $("#btnManagerComments").hide();
            $("#btnHRComments").hide();
            $("#btnShowDetails").show();
        }
    }
    $("#hiddenid").val(EncryptedId);
    $("#LinkPopUp").css({
        top: positionlink.top,
        left: positionlink.left + width,
        position: "absolute"
    }).show();
    $("#btnShowStatus").focus();
}