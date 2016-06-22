$("#TravelShowStatusTable").jqGrid({
    // Ajax related configurations
    url: 'TravelShowStatusDetails/Travel',
    datatype: "json",
    mtype: "POST",
    postData: { TravelId: function () { return $('#travelIdToPassOnLinkClick').val() } },
    // Specify the column names
    colNames: ["Employee Code", "Employee Id", "Employee Name", "Stage ID", "Travel ID", "Stage", "Status", "Time", "Actor", "Action","Comments"],
    // Configure the columns
    colModel: [
                { name: "TravelShowstatusEmployeeCode", index: "TravelShowstatusEmployeeCode", hidden: true, width: 50, align: "left" },
                { name: "TravelShowstatusEmployeeId", index: "TravelShowstatusEmployeeId", hidden: true, width: 50, align: "left" },
                { name: "TravelShowstatusEmployeeName", index: "TravelShowstatusEmployeeName", hidden: true, width: 50, align: "left" },
                { name: "TravelShowstatusStageID", index: "TravelShowstatusStageID", hidden: true, width: 50, align: "left" },
                { name: "ShowstatusTravelId", index: "ShowstatusTravelId", hidden: true, width: 50, align: "left" },
                { name: "TravelShowstatusCurrentStage", index: "TravelShowstatusCurrentStage", width: 50, align: "left" },
                { name: "showStatus", index: "showStatus", width: 100, align: "left" },
                { name: "TravelShowstatusTime", index: "TravelShowstatusTime", width: 50, align: "left", sorttype: 'date', formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'm/d/Y H:i:s'} },
                { name: "TravelShowstatusActor", index: "TravelShowstatusActor", width: 50, align: "left" },
                { name: "TravelShowstatusAction", index: "TravelShowstatusAction", width: 50, align: "left" },
                { name: "TravelShowstatusComments", index: "TravelShowstatusComments", width: 50, align: "left" }
            ],

    height: 400,
    jsonReader: { repeatitems: false },
    toppager: false,
    bottompager: false,
    // Paging
    pager: $("#TravelShowStatusTablePager"),
    rowNum: 10,
    rowList: [10, 20, 40],
    viewrecords: true, // Specify if "total number of records" is displayed
    height: 'auto',
    autowidth: false,
    //caption: "Show Status",
    onCellSelect: function (rowid, iCol) {
        var rowData = $(this).getRowData(rowid);
        var selectedTravelId = rowData['ShowstatusTravelId'];
    }
}).navGrid("#TravelShowStatusTablePager",
                                            { search: false, refresh: true, add: false, edit: false, del: false }
                                         );