$(document).ready(function () {
    //    $("#inbox").click(function () {
    //        $("#gbox_ConfirmationTableInbox").show();
    //        $("#gbox_ConfirmationTableWatchList").hide();
    //    });

    //    $("#watchlist").click(function () {
    //        $("#gbox_ConfirmationTableInboxList").hide();
    //        $("#gbox_ConfirmationTableWatchList").show();
    //    });

    //    $("#all").click(function () {
    //        $("#gbox_ConfirmationTableInboxList").show();
    //        $("#gbox_ConfirmationTableWatchList").show();
    //    });

    $("#btnshowFilter").click(function () {
        //        jQuery("#ConfirmationTableInboxList").trigger("reloadGrid");
        //        jQuery("#ConfirmationTableWatchList").trigger("reloadGrid");
        $("#ConfirmationTableInboxList").jqGrid("setGridParam", { datatype: "json" }).trigger("reloadGrid");
        $("#ConfirmationTableWatchList").jqGrid("setGridParam", { datatype: "json" }).trigger("reloadGrid");
    });

    $("#btnShowStatusConfirmation").click(function () {
        //jQuery("#ConfirmationShowStatusTableNew").trigger("reloadGrid");
        $("#ConfirmationShowStatusTableNew").jqGrid("setGridParam", { datatype: "json" }).trigger("reloadGrid");
        $('#ShowStatusDialogConfirmation').dialog({
            autoOpen: true,
            modal: true,
            width: 1000,
            resizable: true,
            title: "Show Status"
        });
    });

    $("#btnFillFormConfirmation").click(function () {
        var encyptedEmplId = $("#hiddenid").val();
        window.location.href = 'ConfirmationFormDetails/ConfirmationProcess?employeeId=' + encyptedEmplId;
        //window.location.href = 'ConfirmationFormDetails/ConfirmationFormDetails?employeeId=' + encyptedEmplId;
    });
    $("#btnShowDetailsConfirmation").click(function () {
        var encyptedEmplId = $("#hiddenid").val();
        window.location.href = 'ConfirmationFormDetails/ConfirmationProcess?employeeId=' + encyptedEmplId + '&viewDetailsBtn=yes';
        //window.location.href = 'ConfirmationFormDetails/ConfirmationFormDetails?employeeId=' + encyptedEmplId + '&viewDetailsBtn=yes';;
    });

    $("#txtSearchTextConfirmation").autocomplete({
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
    var searchterm = $("#txtSearchTextConfirmation").val();
    SetPageTitle('Initiate Confirmation Process');
    var deletionIdSelected = 0;
    $("#ConfirmationTableInboxList").jqGrid({
        // Ajax related configurations
        url: 'LoadGridConfirmationDetailsList/ConfirmationProcess',
        datatype: "json",
        mtype: "POST",
        postData: {
            term: function () { return $("#txtSearchTextConfirmation").val(); },
            Field: function () { return $("#Field").val(); },
            FieldChild: function () { if ($("#Field").val() == "Business Group") return $("#DDFieldChildListConfBG").val(); if ($("#Field").val() == "Organization Unit") return $("#FieldChildListConfOU").val(); if ($("#Field").val() == "Stage Name") return $("#DDFieldChildListConfSN").val(); if ($("#Field").val() == "") return ""; }
        },
        // Specify the column names
        colNames: ["Confirmation ID", "Employee Code", "Is Manager", "Is Further Approver", "Is Admin", "Employee Id", "Encrypted Employee Id", "Employee Name", "Status", "Joining Date", "Probation Review Date", "Initiated Date", "Stage ID", "Stage", "Stage ActorID", "Stage ActorName", "Field"],
        // Configure the columns
        colModel: [
            { name: "ConfirmationID", index: "ConfirmationID", hidden: true, width: 50, align: "left" },
                { name: "EmployeeCode", index: "EmployeeCode", hidden: true, width: 50, align: "left" },
                { name: "IsManager", index: "IsManager", hidden: true, width: 50, align: "left" },
                { name: "IsFurtherApprover", index: "IsFurtherApprover", hidden: true, width: 50, align: "left" },
                { name: "IsAdmin", index: "IsAdmin", hidden: true, width: 50, align: "left" },
                { name: "EmployeeId", index: "EmployeeId", hidden: true, width: 50, align: "left" },
                { name: "encryptedEmployeeId", index: "encryptedEmployeeId", hidden: true, width: 50, align: "left" },
                { name: "EmployeeName", index: "EmployeeName", width: 50, align: "left", formatter: formatlink },
                { name: "Status", index: "Status", width: 100, align: "left", formatter: RenderImages, classes: 'StatusClass' },
                { name: "JoiningDate", index: "JoiningDate", width: 50, align: "left", formatter: 'date' },
                { name: "ProbationReviewDate", index: "ProbationReviewDate", width: 50, align: "left", formatter: 'date' },
                { name: "InitiatedDate", index: "InitiatedDate", width: 50, align: "left", formatter: 'date' },
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
        loadonce: true,
        jsonReader: { repeatitems: false },
        pager: $("#ConfirmationTablePagerInboxList"),
        rowNum: 5,
        rowList: [5, 10, 20], viewrecords: true, // Specify if "total number of records" is displayed
        height: 'auto',
        autowidth: false,
        sortable: true,
        //caption: "Inbox",
        onCellSelect: function (rowid, iCol) {
            var rowData = $(this).getRowData(rowid);
            var selectedDependantId = rowData['EmployeeId']
        },
        gridComplete: function () {
            $(".StatusClass").removeAttr('title');
        }
    }).navGrid("#ConfirmationTablePagerInboxList",
                                            { search: false, refresh: false, add: false, edit: false, del: false }
                                           );

    $("#Field").change(function () {
        //        if ($("#Field").val() != "") {
        //            var url = 'GetFieldDropdownDetails/ConfirmationProcess';
        //            $("#FieldChild").show();
        //            //fire off the request, passing it the MatserId which is the employeementStatus selected item value
        //            $.getJSON(url, { FieldName: $("#Field").val() }, function (data) {
        //                //Clear the Model list
        //                $("#FieldChild").empty();
        //                $("#FieldChild").append("<option value='" + "" + "'>" + "Select" + "</option>");
        //                //Foreach Model in the list, add a model option from the data returned
        //                $.each(data, function (index, optionData) {
        //                    $("#FieldChild").append("<option value='" + optionData.ID + "'>" + optionData.Discription + "</option>");
        //                });
        //            });
        //        }
        //        else {
        //            $("#FieldChild").val("");
        //            $("#FieldChild").hide();
        //        }
        if ($("#Field").val() != "") {
            $("#Field").val() == "Select"
            {
                $('#FieldChildListConfBG').hide();
                $('#FieldChildListConfOU').hide();
                $('#FieldChildListConfSN').hide();
            }
            if ($("#Field").val() == "Buisness Group") {
                $('#FieldChildListConfBG').show();
                $('#FieldChildListConfOU').hide();
                $('#FieldChildListConfSN').hide();
            }
            if ($("#Field").val() == "Organization Unit") {
                $('#FieldChildListConfBG').hide();
                $('#FieldChildListConfOU').show();
                $('#FieldChildListConfSN').hide();
            }
            if ($("#Field").val() == "Stage Name") {
                $('#FieldChildListConfBG').hide();
                $('#FieldChildListConfOU').hide();
                $('#FieldChildListConfSN').show();
            }
        } else {
            $("#Field").val("");
            $("#Field").hide();
        }
    });
});

function formatlink(cellvalue, options, rowobject) {
    var status = rowobject['Field'];
    var encryptedEmployeeId = rowobject['encryptedEmployeeId'];
    if (rowobject['Field'] == 'Move Ahead')
        status = 'moveahead';
    return "<a href=# id=" + rowobject['EmployeeId'] + " class=EmpLink onClick = LinkClick(this," + rowobject['StageID'] + ",'" + encryptedEmployeeId + "'" + ",'" + status + "'" + ",'" + rowobject['ConfirmationID'] + "'" + ",'" + rowobject['IsManager'] + "'" + ",'" + rowobject['IsFurtherApprover'] + "'" + ",'" + rowobject['IsAdmin'] + "') class=EmployeeNameLink >" + cellvalue + "</a>";
}

function RenderImages(cellvalue, options, rowobject) {
    var obj;
    obj = "";
    if (rowobject['IsFurtherApproverPresent'] == false) {
        for (var i = 1; i < 3; i++) {
            if (rowobject['Field'] == "Reject" && rowobject['StageID'] == i) {
                switch (i) {
                    case 1:
                        obj = obj + "<img src='../../Images/New Design/status-rejected.png' title ='Reporting Manager Stage' width='31px' height='31px' class='StatusImagesRed'> "; //  Red
                        break;

                    case 2:
                        obj = obj + "<img src='../../Images/New Design/status-rejected.png' title='HR Approval Stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                        break;

                    case 3:
                        obj = obj + "<img src='../../Images/New Design/status-rejected.png' title='DU Head stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                        break;

                    case 4:
                        obj = obj + "<img src='../../Images/New Design/status-rejected.png' title='Confirmation Completed' width='31px' height='31px' class='StatusImagesGreen'> ";
                        break;
                    default:

                        obj = obj + "<img src='../../Images/New Design/status-rejected.png' width='31px' height='31px' class='StatusImagesGreen'> "; //  Green
                }
                //  obj = obj + "<img src='../../Images/New Design/status-rejected.png' width='31px' height='31px' class='StatusImagesRed'> "; //  Red
                i++;
            }
            if (rowobject['StageID'] >= i) {
                if (rowobject['Field'] == "Approved" && rowobject['StageID'] == i) {
                    switch (i) {
                        case 1:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='Reporting Manager Stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 2:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='HR Approval Stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 3:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='DU Head stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 4:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='Confirmation Completed' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;
                        default:

                            obj = obj + "<img src='../../Images/New Design/status-on.png' width='31px' height='31px' class='StatusImagesGreen'> "; //  Green
                    }
                }
                else if (rowobject['StageID'] != i) {
                    switch (i) {
                        case 1:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='Reporting Manager Stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 2:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='HR Approval Stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 3:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='DU Head stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 4:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='Confirmation Completed' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;
                        default:

                            obj = obj + "<img src='../../Images/New Design/status-on.png' width='31px' height='31px' class='StatusImagesGreen'> "; //  Green
                    }
                }
                else {
                    if (rowobject['StageID'] == 4) {
                        obj = obj + "<img src='../../Images/New Design/status-on.png' title='Confirmation Completed' width='31px' height='31px' class='StatusImagesGreen'> "; // Green
                    }
                    else {
                        obj = obj + "<img src='../../Images/New Design/status-off.png'  width='31px' height='31px' class='StatusImagesGray'>"; //  Yellow
                    }
                }
            } else {
                obj = obj + "<img src='../../Images/New Design/status-off.png'  width='31px' height='31px' class='StatusImagesGray'>"; //  Yellow
            }
        }
    }
    else {
        if (rowobject['StageID'] == 1 && rowobject['IsFurtherApproverCleared'] == true) {
            rowobject['StageID'] = 3;
        }
        for (var i = 1; i < 5; i++) {
            if (rowobject['Field'] == "Rejected" && rowobject['StageID'] == (i - 1)) {
                switch (i) {
                    case 1:
                        obj = obj + "<img src='../../Images/New Design/status-rejected.png' title ='Reporting Manager Stage' width='31px' height='31px' class='StatusImagesRed'> "; //  Red
                        break;

                    case 2:
                        obj = obj + "<img src='../../Images/New Design/status-rejected.png' title='HR Approval Stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                        break;

                    case 3:
                        obj = obj + "<img src='../../Images/New Design/status-rejected.png' title='DU Head stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                        break;

                    case 4:
                        obj = obj + "<img src='../../Images/New Design/status-rejected.png' title='Confirmation Completed' width='31px' height='31px' class='StatusImagesGreen'> ";
                        break;
                    default:

                        obj = obj + "<img src='../../Images/New Design/status-rejected.png' width='31px' height='31px' class='StatusImagesGreen'> "; //  Green
                }
                //  obj = obj + "<img src='../../Images/New Design/status-rejected.png' width='31px' height='31px' class='StatusImagesRed'> "; //  Red
                i++;
            }
            if (rowobject['StageID'] >= i) {
                if (rowobject['Field'] == "Approved" && rowobject['StageID'] == i) {
                    switch (i) {
                        case 1:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='Reporting Manager Stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 2:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='HR Approval Stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 3:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='DU Head stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 4:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='Confirmation Completed' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;
                        default:

                            obj = obj + "<img src='../../Images/New Design/status-on.png' width='31px' height='31px' class='StatusImagesGreen'> "; //  Green
                    }
                    //obj = obj + "<img src='../../Images/New Design/status-on.png' width='31px' height='31px' class='StatusImagesGreen'> "; //  Green
                }
                else if (rowobject['StageID'] != i) {
                    switch (i) {
                        case 1:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='Reporting Manager Stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 2:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='HR Approval Stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 3:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='DU Head stage' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;

                        case 4:
                            obj = obj + "<img src='../../Images/New Design/status-on.png' title='Confirmation Completed' width='31px' height='31px' class='StatusImagesGreen'> ";
                            break;
                        default:

                            obj = obj + "<img src='../../Images/New Design/status-on.png' width='31px' height='31px' class='StatusImagesGreen'> "; //  Green
                    }
                    //   obj = obj + "<img src='../../Images/New Design/status-on.png' width='31px' height='31px' class='StatusImagesGreen'> "; //  Green
                }
                else {
                    if (rowobject['StageID'] == 4) {
                        obj = obj + "<img src='../../Images/New Design/status-on.png' title='Confirmation Completed' width='31px' height='31px' class='StatusImagesGreen'> "; //  Green
                    }
                    else {
                        obj = obj + "<img src='../../Images/New Design/status-off.png' width='31px' height='31px' class='StatusImagesGray'>"; //  Yellow
                    }
                }
            } else {
                obj = obj + "<img src='../../Images/New Design/status-off.png'  width='31px' height='31px' class='StatusImagesGray'>"; //  Yellow
            }
        }
        if (rowobject['StageID'] == 1 && rowobject['IsFurtherApproverCleared'] == true) {
            rowobject['StageID'] = 1;
        }
    }
    return obj;
}
function LinkClick(event, StageId, EncryptedId, ActionType, ConfirmationID, IsManager, IsFurtherApprover, IsAdmin) {
    $("#hdnConfirmationId").val(ConfirmationID);
    var reject = ActionType;
    var target = event.id;
    var width = $("#" + target + "").width();
    var positionlink = $("#" + target + "").offset();
    var table = $("#" + target + "").parent().parent().parent().parent();
    var tableid = $(table).attr('id');
    if (IsManager == "true" && (StageId == null || StageId == 0)) {
        $("#btnFillFormConfirmation").show();
        $("#btnShowDetailsConfirmation").hide();
    }
    else if (IsFurtherApprover == "true" && StageId == 2) {
        $("#btnFillFormConfirmation").show();
        $("#btnShowDetailsConfirmation").hide();
    }
    else if (IsAdmin == "true" && (StageId == 1 || StageId == 3 || StageId == 0)) {
        $("#btnFillFormConfirmation").show();
        $("#btnShowDetailsConfirmation").hide();
    }
    else {
        $("#btnFillFormConfirmation").hide();
        $("#btnShowDetailsConfirmation").show();
    }
    $("#hiddenid").val(EncryptedId);
    $("#LinkPopUpConfirmation").css({
        top: positionlink.top,
        left: positionlink.left + width,
        position: "fixed"
    }).show();
    $("#btnShowStatusConfirmation").focus();
}