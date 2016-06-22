function SeparationshowStatus() {
	$("#ShowSeparationStatusTable").jqGrid({
		// Ajax related configurations
		url: "SeparationStatusDetailsLoadGrid/Exit",
		datatype: "json",
		mtype: "POST",
		postData: { exitInstanceId: function () { return $("#exitIdToPassOnLinkClick").val(); } },
		// Specify the column names
		colNames: ["Employee Code", "Employee Id", "Employee Name", "Stage ID", "Stage", "Status", "Time", "Actor", "Action", "Comments"],
		// Configure the columns
		colModel: [
              { name: "ShowstatusEmployeeCode", index: "ShowstatusEmployeeCode", hidden: true, width: 10, align: "left" },
             { name: "ShowstatusEmployeeId", index: "ShowstatusEmployeeId", hidden: true, width: 10, align: "left" },
              { name: "ShowstatusEmployeeName", index: "ShowstatusEmployeeName", hidden: true, width: 10, align: "left" },
              { name: "ShowstatusStageID", index: "ShowstatusStageID", hidden: true, width: 10, align: "left" },
               { name: "ShowstatusCurrentStage", index: "ShowstatusCurrentStage", width: 40, align: "left" },
              { name: "showStatus", index: "showStatus", width: 80, align: "left" },
              { name: "ShowstatusTime", index: "ShowstatusTime", width: 30, align: "left", sorttype: 'datetime', formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'm/d/Y H:i:s'} },
             { name: "ShowstatusActor", index: "ShowstatusActor", width: 30, align: "left" },
             { name: "ShowstatusAction", index: "ShowstatusAction", width: 20, align: "left" },
             { name: "ShowstatusComments", index: "ShowstatusComments", width: 50, align: "left" },

            ],

		width: 1100,
		height: 400,
		// Paging
		toppager: false,
		jsonReader: { repeatitems: false },
		pager: $("#ShowStatusTablePager"),
		rowNum: 10,
		rowList: [10, 20, 40],
		viewrecords: true, // Specify if "total number of records" is displayed
		height: 'auto',
		autowidth: false,
		gridComplete: function () {
		    $('.ui-pg-table').find('select').selectBox("detach");
		},
		//caption: "Employee Separation Process Status",
		onCellSelect: function (rowid, iCol) {
			var rowData = $(this).getRowData(rowid);
			var selectedDependantId = rowData['EmployeeId'];
			//                    if (iCol == 0) {
			//                        InitiateProcess(rowData);
			//                    }
		}
	}).navGrid("#ShowStatusTablePager",
                                            { search: false, refresh: false, add: false, edit: false, del: false },
                                            {},
                                            {},
                                            {}
                                         );
}