      $("#txtSearchTextConfrm").autocomplete({
            source: function (request, response) {
                $.getJSON('SearchEmployeeAutoSuggestCnfrm/ConfirmationProcess', { term: request.term }, function (data) {
                    response($.map(data, function (el) {
                        var searchterm = $("#txtSearchTextConfrm").val();
                        var emplyeeInformation = el.EmployeeName + " {" + el.EmployeeCode + "}";
                        return {
                            label: emplyeeInformation,
                            value: el.EmployeeName
                        };
                    }));
                });
            }
        });
        var searchterm = $("#txtSearchTextConfrm").val();
        SetPageTitle('Initiate Confirmation Process');
        var deletionIdSelected = 0;
        // Set up the jquery grid
        $("#ConfigurationTable").jqGrid({
            // Ajax related configurations
            url: 'InitiateConfirmationLoadGrid/ConfirmationProcess',
            datatype: "json",
            mtype: "POST",
            postData: { term: function () { return $("#txtSearchTextConfrm").val(); } },
            // Specify the column names
            colNames: ["Initiate", "Employee Code", "Employee Id","Encrypted Employee Id", "Employee Name", "ConfirmationStatus", "Reporting Manager", "Reporting Manager 2", "Reviewer", "HR Reviewer", "Joining Date", "Probation Review Date", "Role", "Business Group", "Grade", "Organization Unit", "Current Stage"],
            // Configure the columns
            colModel: [
             { name: "Initiate", index: "Initiate", width: 71, formatter: checkstatus, align: 'left' },
              { name: "EmployeeCode", index: "EmployeeCode", width: 91, align: "left" },
             { name: "EmployeeId", index: "EmployeeId", hidden: true, width: 50, align: "left" },
              { name: "encryptedEmployeeId", index: "encryptedEmployeeId", hidden: true, width: 50, align: "left" },
              { name: "EmployeeName", index: "EmployeeName", width: 91, align: "left" },
               { name: "ConfirmationStatus", index: "ConfirmationStatus", hidden: true, width: 50, align: "left" },
              { name: "ReportingManager", index: "ReportingManager", width: 87, align: "left" },
               { name: "ReportingManager2", index: "ReportingManager2", width: 87, align: "left" },
                { name: "Reviewer", index: "Reviewer", width: 85, align: "left" },
                 { name: "HRReviewer", index: "HRReviewer", width: 85, align: "left" },
                  { name: "JoiningDate", index: "JoiningDate", width: 88, align: "left", sorttype: 'date', formatter: 'date' },
                       { name: "ProbationReviewDate", index: "ProbationReviewDate", width: 88, align: "left", sorttype: 'date', formatter: 'date', formatoptions: { newformat: 'm/d/Y'} },
                    { name: "RoleInitiate", index: "RoleInitiate", width: 60, align: "left" },
	        { name: "BusinessGroup", index: "BusinessGroup", width: 90, align: "left" },
             { name: "Grade", index: "Grade", width: 55, align: "left" },
              { name: "OrganizationUnit", index: "OrganizationUnit", width: 114, align: "left" },
              { name: "CurrentStage", index: "CurrentStage", width: 75, align: "left", formatter: checkstage }
            ],
            width: 940,
            height: 200,
            // Paging
            toppager: true,
            jsonReader: { repeatitems: false },
            pager: $("#ConfigurationTablePager"),
            rowNum: 5,
            rowList: [5, 10, 20],
            viewrecords: true, // Specify if "total number of records" is displayed
            height: 'auto',
            autowidth: false,
            caption: "Confirmation Details",
            onCellSelect: function (rowid, iCol) {
                var rowData = $(this).getRowData(rowid);
                var selectedDependantId = rowData['EmployeeId']
                var cm = $(this).jqGrid("getGridParam", "colModel");
                var colName = cm[iCol];
                if (colName['index'] == 'Initiate') {
                    InitiateProcess(rowData);
                }
            }
        }).navGrid("#ConfigurationTablePager",
                            { search: false, refresh: false, add: false, edit: false, del: false }
                                           );

    function checkstage(cellvalue, options, rowobject) {
        var stage = rowobject['CurrentStage'];
        if (stage == null) {
            return 'Not Initiated';
        }
        else {
            return stage;
        }
    }
    function checkstatus(cellvalue, options, rowobject) {
        var CnfStatus = rowobject['ConfirmationStatus'];
         //if (stage == null || stage=='Initiate Confirmation' ) {
        if (CnfStatus == null || CnfStatus == 0) {
            return '<span style="color:blue;text-decoration:underline;cursor:pointer;">Initiate</span>';
        }
        else {
            return '<span>-</span>';
        }
    }

    function InitiateProcess(Object) {
        $(".field-validation-error").empty();
        $('input').removeClass("input-validation-error");
        var CnfStatus = Object['ConfirmationStatus'];
        //if (Object['CurrentStage'] == "Not Initiated" || Object['CurrentStage'] == 'Initiate Confirmation') {
        if (Object['CurrentStage'] == "Not Initiated" || CnfStatus == null || CnfStatus == 0) {
            var d = new Date();
            var month = d.getMonth() + 1;
            var day = d.getDate();
            var systemdate = (day < 10 ? '0' : '') + day + '/' +
(month < 10 ? '0' : '') + month + '/' +
d.getFullYear();
            $('#hdnInitiateConfirmationEmployeeId').val(Object['encryptedEmployeeId']);
            $('#hdnInitiateConfirmationEmployeeCode').val(Object['EmployeeCode']);
            $('#EmployeeName').val(Object['EmployeeName']);
            $('#InitiationDate').val(systemdate);
            $('#ConfirmationCoordinator').val('');
            $('#Comments').val();
            $("#ConfirmInitiate").dialog({
                modal: true,
                width: 610,
                height: 600,
                resizable: false,
                title: "Initiate Process",
                open: function (event, ui) {
                    $("#ConfirmInitiate").load("/ConfirmationProcess/ConfirmInitiate?initiateEmpID=" + Object['encryptedEmployeeId']).dialog("open");
                }
            });
        }
    }

     $("#txtSearchTextConfrm").keypress(function (e) {
        if (e.keyCode == 13) {
            searchterm = $("#txtSearchTextConfrm").val();
            jQuery("#ConfigurationTable").trigger("reloadGrid");
        }
    });