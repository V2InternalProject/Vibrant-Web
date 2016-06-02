$("#ReimbursementShowStatusTable").jqGrid({
    // Ajax related configurations
    url: 'ShowStatusDetails/ExpenseReimbursement',
    datatype: "json",
    mtype: "POST",
    postData: { ExpenseId: function () { return $('#exenseIdToPassOnLinkClick').val() } },
    // Specify the column names
    colNames: ["Employee Code", "Employee Id", "Employee Name", "Stage ID", "Stage", "Status", "Time", "Actor", "Action", "Comments"],
    // Configure the columns
    colModel: [
                { name: "ShowstatusEmployeeCode", index: "ShowstatusEmployeeCode", hidden: true, width: 50, align: "left" },
                { name: "ShowstatusEmployeeId", index: "ShowstatusEmployeeId", hidden: true, width: 50, align: "left" },
                { name: "ShowstatusEmployeeName", index: "ShowstatusEmployeeName", hidden: true, width: 50, align: "left" },
                { name: "ShowstatusStageID", index: "ShowstatusStageID", hidden: true, width: 50, align: "left" },
                { name: "ShowstatusCurrentStage", index: "ShowstatusCurrentStage", width: 50, align: "left" },
                { name: "showStatus", index: "showStatus", width: 100, align: "left" },
                { name: "ShowstatusTime", index: "ShowstatusTime", width: 50, align: "left", sorttype: 'date', formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'm/d/Y H:i:s'} },
                { name: "ShowstatusActor", index: "ShowstatusActor", width: 50, align: "left" },
                { name: "ShowstatusAction", index: "ShowstatusAction", width: 50, align: "left" },
                { name: "ShowstatusComments", index: "ShowstatusComments", width: 50, align: "left" }
            ],

    height: 400,
    jsonReader: { repeatitems: false },
    toppager: false,
    bottompager: false,
    // Paging
    pager: $("#ShowStatusReimbusTablePager"),
    rowNum: 10,
    rowList: [10, 20, 40],
    viewrecords: true, // Specify if "total number of records" is displayed
    height: 'auto',
    autowidth: false,
    //caption: "Show Status",
    onCellSelect: function (rowid, iCol) {
        var rowData = $(this).getRowData(rowid);
        var selectedDependantId = rowData['EmployeeId'];
    }
}).navGrid("#ShowStatusReimbusTablePager",
                                            { search: false, refresh: true, add: false, edit: false, del: false }
                                         );