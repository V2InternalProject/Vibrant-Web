/* File Created: August 14, 2013 */

$.ajaxSetup({
    // Disable caching of AJAX responses
    cache: false
});

$(document).ready(function () {
    // Set up the jquery grid
    $("#jqProjectDetailsTable").jqGrid({
        // Ajax related configurations
        url: window.loadprojectUrl,
        postData: { employeeId: window.encryptedEmployeeId },

        datatype: "json",
        mtype: "POST",

        // Specify the column names
        colNames: ["Employee Id", "Project resource Id", "Project Detail Id", "Allocation Start Date", "Allocation End Date", "Current Role", "Current Project", "Current Reporting Manager", "Delivery Unit", "Resource Pool Name", "Resource Pool Manager"],

        // Configure the columns
        colModel: [
	            { name: "EmployeeId", index: "EmployeeId", hidden: true, width: 100, align: "left",editable:false },
	            { name: "ProjectResourceID", index: "ProjectResourceID", hidden: true, width: 100, align: "left" ,editable:false},
	            { name: "ProjectDetailID", index: "ProjectDetailID", hidden: true, width: 100, align: "left", editable: false },
	            { name: "FromDate", index: "FromDate", width: 180,editable:false, align: "left", sorttype: 'date', formatter: 'date', formatoptions: { newformat: 'm/d/Y' } },
	            { name: "ToDate", index: "ToDate", width: 180,editable:false, align: "left", sorttype: 'date', formatter: 'date', formatoptions: { newformat: 'm/d/Y' } },
	            { name: "CurrentRole", index: "CurrentRole",editable:false, width: 250, align: "left" },
	            { name: "CurrentProject", index: "CurrentProject",editable:false, width: 250, align: "left" },
	            { name: "CurrentReportingManager", index: "CurrentReportingManager",editable:false, width: 300, align: "left" },
	            { name: "DeliveryUnit", index: "DeliveryUnit",editable:false, width: 250, align: "left" },
	            { name: "ResourcePoolName", index: "ResourcePoolName",editable:false, width: 375, align: "left" },
	            { name: "ResourcePoolManager", index: "ResourcePoolManager", editable:false,width: 375, align: "left" },
        ],

        width: 800,

        // Paging
        toppager: false,
        jsonReader: { repeatitems: false },
        pager: $("#jqProjectTablePager"),
        rowNum: 20,
        rowList: [],
        viewrecords: true, // Specify if "total number of records" is displayed
        height: 'auto',
        autowidth: false,
        //caption: "Employee - Project Details",
        gridComplete: function () {
            var grid = $(this).getRowData();
            var count = jQuery("#jqProjectDetailsTable").jqGrid('getGridParam', 'records');
            //if (count == 0) {
            //    $('#gbox_jqProjectDetailsTable').hide();
            //}
            //else {
            //    $('#gbox_jqProjectDetailsTable').show();
            //    if (count > 20) {
            //        $('#jqProjectTablePager').show();
            //    }
            //    else {
            //        $('#jqProjectTablePager').hide();
            //    }
            //}

            if (count == 0) {
                $('#gbox_jqProjectDetailsTable').hide();
                $("#ProjectDetailsContainer").append("<p>No records found</p>")
            }

            if (window.Empstatusmasterid == 2) {
                $("#gbox_jqProjectDetailsTable").find('input,select').attr("disabled", true);
                ("#jqProjectDetailsTable").jqGrid('hideCol', 'Delete');
                //$("jqTableBondDetailsPager_left").css("visibility", "hidden");
            }
        },
        onCellSelect: function (rowid, iCol) {
            if ($('#UserRole').val() != window.HRAdmin) {
                return false;
            }
        }
    }).navGrid("#jqProjectTablePager",
	        { refresh: false, add: false, edit: false, del: false, search: false }
	    );

    //if ($('#UserRole').val() != HRAdmin) {
    //    $("#jqTableBondDetailsPager_left").css("visibility", "hidden");
    //    $("#jqBondDetailsTable").jqGrid('hideCol', 'Delete');
    //}
});// ready end