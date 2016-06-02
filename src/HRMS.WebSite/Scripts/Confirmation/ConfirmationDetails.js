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
    //Disable caching and asyn of AJAX responses.
    $.ajaxSetup({ async: false }, { cache: false });

    $.validator.unobtrusive.parse($("#frmGoalAquire"));
    $.validator.unobtrusive.parse($("#frmConfirmationDetails"));
    $.validator.unobtrusive.parse($("#frmAddPerfHinderDetails"));
    $.validator.unobtrusive.parse($("#frmValueDriver"));
    $('#ConfirmationDate').datepicker({ dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, minDate: new Date, yearRange: "c:+10", showOn: "both", buttonImage: "../../Content/themes/base/images/calendar.gif", buttonImageOnly: true });
    $('#PIPDate').datepicker({ dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, minDate: new Date, yearRange: "c:+10", showOn: "both", buttonImage: "../../Content/themes/base/images/calendar.gif", buttonImageOnly: true });
    $('#ExtendProbationDate').datepicker({ dateFormat: "mm/dd/yy", changeMonth: true, changeYear: true, minDate: new Date, yearRange: "c:+10", showOn: "both", buttonImage: "../../Content/themes/base/images/calendar.gif", buttonImageOnly: true });

    if ($('#IsManagerOrEmployee').val() == "Employee") {
        $('.approveReject').hide();
    }
    $("#IsAcceptedOrExtended").val('accept');
    $("#acceptConfirmation").click(function () {
        $("#IsAcceptedOrExtended").val('accept');
        $("#ConfirmationAccept").show();
        $("#ExtendConfirmation").hide();
        $("#TerminateConfirmation").hide();
    });

    $("#extendProbation").click(function () {
        $("#IsAcceptedOrExtended").val('extend');
        $("#ExtendConfirmation").show();
        $("#ConfirmationAccept").hide();
        $("#TerminateConfirmation").hide();
    });

    $("#termination").click(function () {
        $("#IsAcceptedOrExtended").val('sendPIP');
        $("#ExtendConfirmation").hide();
        $("#ConfirmationAccept").hide();
        $("#TerminateConfirmation").show();
    });

    if ($('#IsManagerOrEmployee').val() != "Employee") {
        $('#SkillDevPrgm').removeAttr('disabled');
        $('#LongTerm').removeAttr('disabled');
        $('#ShortTerm').removeAttr('disabled');
        $('#reject').removeAttr('disabled');
    }

    if ($('#IsManagerOrEmployee').val() == "Employee") {
        /* below id's r for perfHinder*/
        $('#empCommentsFFSelf').removeAttr('disabled');
        $('#empCommentsFFEnvi').removeAttr('disabled');
        $('#empCommentsIFSelf').removeAttr('disabled');
        $('#empCommentsIFEnvi').removeAttr('disabled');
        $('#empCommentsSupport').removeAttr('disabled');
        if (isClickedViewDetails == "yes") {
            $('#empCommentsFFSelf').attr('disabled', true);
            $('#empCommentsFFEnvi').attr('disabled', true);
            $('#empCommentsIFSelf').attr('disabled', true);
            $('#empCommentsIFEnvi').attr('disabled', true);
            $('#empCommentsSupport').attr('disabled', true);
        }
        $('#tdReviewerCommentsFFSelf').show();
        $('#tdReviewerCommentsFFEnvi').show();
        $('#tdReviewerCommentsIFSelf').show();
        $('#tdReviewerCommentsIFEnvi').show();
        $('#tdReviewerCommentsSupport').show();
        $('#thReviewComments').show();
        $('#tdHrCommentsFFSelf').show();
        $('#tdHrCommentsFFEnvi').show();
        $('#tdHrCommentsIFSelf').show();
        $('#tdHrCommentsIFEnvi').show();
        $('#tdHrCommentsSupport').show();
        $('#thHRComments').show();
    }
    if ($('#IsManagerOrEmployee').val() == "Manager") {
        $('#btnAddSkillAquiredDetails').hide();
        $('#btnAddProjectAchievementsDetails').hide();
        $('#btnAddCorporateDetails').hide();
        $('#btnAddQualificationDetails').hide();
        $('#mngrCommentsFFSelf').removeAttr('disabled');
        $('#mngrCommentsFFEnvi').removeAttr('disabled');
        $('#mngrCommentsIFSelf').removeAttr('disabled');
        $('#mngrCommentsIFEnvi').removeAttr('disabled');
        $('#mngrCommentsSupport').removeAttr('disabled');
        $('#reject').removeAttr('disabled');
    }
    if ($('#IsManagerOrEmployee').val() == "Manager2") {
        $('#btnAddSkillAquiredDetails').hide();
        $('#btnAddProjectAchievementsDetails').hide();
        $('#btnAddCorporateDetails').hide();
        $('#btnAddQualificationDetails').hide();
        $('#mngrCommentsFFSelfSecond').removeAttr('disabled');
        $('#mngrCommentsFFEnviSecond').removeAttr('disabled');
        $('#mngrCommentsIFSelfSecond').removeAttr('disabled');
        $('#mngrCommentsIFEnviSecond').removeAttr('disabled');
        $('#mngrCommentsSupportSecond').removeAttr('disabled');
        $('#reject').removeAttr('disabled');
        if (isClickedViewDetails == "yes") {
            $('#mngrCommentsFFSelfSecond').attr('disabled', 'disabled');
            $('#mngrCommentsFFEnviSecond').attr('disabled', 'disabled');
            $('#mngrCommentsIFSelfSecond').attr('disabled', 'disabled');
            $('#mngrCommentsIFEnviSecond').attr('disabled', 'disabled');
            $('#mngrCommentsSupportSecond').attr('disabled', 'disabled');
            $('#reject').attr('disabled', 'disabled');
        }
    }

    if ($('#IsManagerOrEmployee').val() == "Reviewer") {
        $('#btnAddSkillAquiredDetails').hide();
        $('#btnAddProjectAchievementsDetails').hide();
        $('#btnAddCorporateDetails').hide();
        $('#btnAddQualificationDetails').hide();
        $('#thReviewComments').show();
        $('#tdReviewerCommentsFFSelf').show();
        $('#tdReviewerCommentsFFEnvi').show();
        $('#tdReviewerCommentsIFSelf').show();
        $('#tdReviewerCommentsIFEnvi').show();
        $('#tdReviewerCommentsSupport').show();
        $('#thReviewComments').show();
        $('#tdHrCommentsFFSelf').show();
        $('#tdHrCommentsFFEnvi').show();
        $('#tdHrCommentsIFSelf').show();
        $('#tdHrCommentsIFEnvi').show();
        $('#tdHrCommentsSupport').show();
        $('#thHRComments').show();
        $('#hrCommentsFFSelf').attr('disabled', true);
        $('#hrCommentsFFEnvi').attr('disabled', true);
        $('#hrCommentsIFSelf').attr('disabled', true);
        $('#hrCommentsIFEnvi').attr('disabled', true);
        $('#hrCommentsSupport').attr('disabled', true);
        $('#reviewerCommentsFFSelf').attr('disabled', false);
        $('#reviewerCommentsFFEnvi').attr('disabled', false);
        $('#reviewerCommentsIFSelf').attr('disabled', false);
        $('#reviewerCommentsIFEnvi').attr('disabled', false);
        $('#reviewerCommentsIFEnvi').attr('disabled', false);
        $('#reviewerCommentsSupport').attr('disabled', false);
        $('#reject').attr('disabled', false);
        if (isClickedViewDetails == "yes") {
            $('#reviewerCommentsFFSelf').attr('disabled', 'disabled');
            $('#reviewerCommentsFFEnvi').attr('disabled', 'disabled');
            $('#reviewerCommentsIFSelf').attr('disabled', 'disabled');
            $('#reviewerCommentsIFEnvi').attr('disabled', 'disabled');
            $('#reviewerCommentsIFEnvi').attr('disabled', 'disabled');
            $('#reviewerCommentsSupport').attr('disabled', 'disabled');
            $('#reject').attr('disabled', 'disabled');
        }
    }
    if ($('#IsManagerOrEmployee').val() == "HR" && $('#StageID').val() == 5) {
        $('#btnAddSkillAquiredDetails').hide();
        $('#btnAddProjectAchievementsDetails').hide();
        $('#btnAddCorporateDetails').hide();
        $('#btnAddQualificationDetails').hide();
        $('#tdReviewerCommentsFFSelf').show();
        $('#tdReviewerCommentsFFEnvi').show();
        $('#tdReviewerCommentsIFSelf').show();
        $('#tdReviewerCommentsIFEnvi').show();
        $('#tdReviewerCommentsIFEnvi').show();
        $('#tdReviewerCommentsSupport').show();
        $('#thReviewComments').show();
        $('#tdHrCommentsFFSelf').show();
        $('#tdHrCommentsFFEnvi').show();
        $('#tdHrCommentsIFSelf').show();
        $('#tdHrCommentsIFEnvi').show();
        $('#tdHrCommentsSupport').show();
        $('#thHRComments').show();
        $('#hrCommentsFFSelf').attr('disabled', false);
        $('#hrCommentsFFEnvi').attr('disabled', false);
        $('#hrCommentsIFSelf').attr('disabled', false);
        $('#hrCommentsIFEnvi').attr('disabled', false);
        $('#hrCommentsSupport').attr('disabled', false);
        $('#reject').attr('disabled', false);
        if (isClickedViewDetails == "yes") {
            $('#hrCommentsFFSelf').attr('disabled', 'disabled');
            $('#hrCommentsFFEnvi').attr('disabled', 'disabled');
            $('#hrCommentsIFSelf').attr('disabled', 'disabled');
            $('#hrCommentsIFEnvi').attr('disabled', 'disabled');
            $('#hrCommentsSupport').attr('disabled', 'disabled');
            $('#reject').attr('disabled', 'disabled');
        }
    }

    if ($('#IsManagerOrEmployee').val() == "HR" && $('#StageID').val() == 6) {
        /* below id's r for perfHinder*/
        $('#btnAddSkillAquiredDetails').hide();
        $('#btnAddProjectAchievementsDetails').hide();
        $('#btnAddCorporateDetails').hide();
        $('#btnAddQualificationDetails').hide();
        $('#tdReviewerCommentsFFSelf').show();
        $('#tdReviewerCommentsFFEnvi').show();
        $('#tdReviewerCommentsIFSelf').show();
        $('#tdReviewerCommentsIFEnvi').show();
        $('#tdReviewerCommentsIFEnvi').show();
        $('#tdReviewerCommentsSupport').show();
        $('#thReviewComments').show();
        $('#tdHrCommentsFFSelf').show();
        $('#tdHrCommentsFFEnvi').show();
        $('#tdHrCommentsIFSelf').show();
        $('#tdHrCommentsIFEnvi').show();
        $('#tdHrCommentsSupport').show();
        $('#thHRComments').show();
        $('#hrCommentsFFSelf').attr('disabled', true);
        $('#hrCommentsFFEnvi').attr('disabled', true);
        $('#hrCommentsIFSelf').attr('disabled', true);
        $('#hrCommentsIFEnvi').attr('disabled', true);
        $('#hrCommentsSupport').attr('disabled', true);
        $('.approveReject').hide();
    }
    if (isClickedViewDetails == "yes") {
        $('#btnApprove').hide();
        $('#btnSave').hide();
        $('.button').hide();
        $('.approveReject').hide();
        $('.ui-datepicker-trigger').hide();
        $('#frmConfirmationDetails').find('input,select,textarea').attr('disabled', true);
        $('#frmAddPerfHinderDetails').find('input,tr,textarea').attr('disabled', true);
        $('#frmGoalAquire').find('input').attr('disabled', true);
        $('#frmValueDriver').find('input,tr,textarea').attr('disabled', true);
        $('#addCorporateContributionDetails').find('input,textarea').attr('disabled', true);
        $('#addAddQualificationDetails').find('input,textarea').attr('disabled', true);
        $('#frmProjectAchievementDetails').find('input,textarea').attr('disabled', true);
    }
    if ($('#StageID').val() == 7) {
        $('.mngrViewDetails').show();
        $('.revViewDetails').show();
    }

    //     /*start of hr closure*/
    if ($('#StageID').val() == 6 && isClickedViewDetails != "yes") {
        $('#empStatus').rules("add", {
            required: function () {
                return ($('#empStatus').val() == ' ' || $('#empStatus').val() == '');
            },
            messages:
                                        {
                                            required: "Employee Status is required."
                                        }
        });

        //$('#empType').rules("add", {
        //    required: function () {
        //        return ($('#empType').val() == ' ' || $('#empType').val() == '');
        //    },
        //    messages:
        //                    			{
        //                    			    required: "Employee Type is required."
        //                    			}
        //});

        $('#gradeName').rules("add", {
            required: function () {
                return ($('#gradeName').val() == ' ' || $('#gradeName').val() == '');
            },
            messages:
                                        {
                                            required: "Grade is required."
                                        }
        });

        $('#roleName').rules("add", {
            required: function () {
                return ($('#roleName').val() == ' ' || $('#roleName').val() == '');
            },
            messages:
                                        {
                                            required: "Role is required."
                                        }
        });

        $('#ConfirmationComments').rules("add", {
            required: function () {
                return ($('#ConfirmationComments').val() == ' ' || $('#ConfirmationComments').val() == '');
            },
            messages:
                                    {
                                        required: "Confirmation Comments is required."
                                    }
        });

        $('#ProbationComments').rules("add", {
            required: function () {
                return ($('#ProbationComments').val() == ' ' || $('#ProbationComments').val() == '');
            },
            messages:
                                    {
                                        required: "Extended Probation Comments is required."
                                    }
        });

        $('#PIPComments').rules("add", {
            required: function () {
                return ($('#PIPComments').val() == ' ' || $('#PIPComments').val() == '');
            },
            messages:
                                    {
                                        required: "PIP Comments is required."
                                    }
        });
    }
    //     /*end of hr closure*/
    /*start of perf hinder*/
    $('#empCommentsFFSelf').rules("add", {
        required: function () {
            return ($('#empCommentsFFSelf').val() == ' ' || $('#empCommentsFFSelf').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#mngrCommentsFFSelf').rules("add", {
        required: function () {
            return ($('#mngrCommentsFFSelf').val() == ' ' || $('#mngrCommentsFFSelf').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#mngrCommentsFFSelfSecond').rules("add", {
        required: function () {
            return ($('#mngrCommentsFFSelfSecond').val() == ' ' || $('#mngrCommentsFFSelfSecond').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#reviewerCommentsFFSelf').rules("add", {
        required: function () {
            return ($('#reviewerCommentsFFSelf').val() == ' ' || $('#reviewerCommentsFFSelf').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#empCommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#empCommentsFFEnvi').val() == ' ' || $('#empCommentsFFEnvi').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#mngrCommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#mngrCommentsFFEnvi').val() == ' ' || $('#mngrCommentsFFEnvi').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#mngrCommentsFFEnviSecond').rules("add", {
        required: function () {
            return ($('#mngrCommentsFFEnviSecond').val() == ' ' || $('#mngrCommentsFFEnviSecond').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#reviewerCommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#reviewerCommentsFFEnvi').val() == ' ' || $('#reviewerCommentsFFEnvi').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });

    $('#empCommentsIFSelf').rules("add", {
        required: function () {
            return ($('#empCommentsIFSelf').val() == ' ' || $('#empCommentsIFSelf').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#mngrCommentsIFSelf').rules("add", {
        required: function () {
            return ($('#mngrCommentsIFSelf').val() == ' ' || $('#mngrCommentsIFSelf').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#mngrCommentsIFSelfSecond').rules("add", {
        required: function () {
            return ($('#mngrCommentsIFSelfSecond').val() == ' ' || $('#mngrCommentsIFSelfSecond').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#reviewerCommentsIFSelf').rules("add", {
        required: function () {
            return ($('#reviewerCommentsIFSelf').val() == ' ' || $('#reviewerCommentsIFSelf').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#hrCommentsIFSelf').rules("add", {
        required: function () {
            return ($('#hrCommentsIFSelf').val() == ' ' || $('#hrCommentsIFSelf').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#empCommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#empCommentsIFEnvi').val() == ' ' || $('#empCommentsIFEnvi').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#mngrCommentsSupport').rules("add", {
        required: function () {
            return ($('#mngrCommentsSupport').val() == ' ' || $('#mngrCommentsSupport').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#mngrCommentsSupportSecond').rules("add", {
        required: function () {
            return ($('#mngrCommentsSupportSecond').val() == ' ' || $('#mngrCommentsSupportSecond').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#reviewerCommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#reviewerCommentsIFEnvi').val() == ' ' || $('#reviewerCommentsIFEnvi').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#hrCommentsSupport').rules("add", {
        required: function () {
            return ($('#hrCommentsSupport').val() == ' ' || $('#hrCommentsSupport').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });

    $('#empCommentsSupport').rules("add", {
        required: function () {
            return ($('#empCommentsSupport').val() == ' ' || $('#empCommentsSupport').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#mngrCommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#mngrCommentsIFEnvi').val() == ' ' || $('#mngrCommentsIFEnvi').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#mngrCommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#mngrCommentsIFEnvi').val() == ' ' || $('#mngrCommentsIFEnvi').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#mngrCommentsIFEnviSecond').rules("add", {
        required: function () {
            return ($('#mngrCommentsIFEnviSecond').val() == ' ' || $('#mngrCommentsIFEnviSecond').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });

    $('#reviewerCommentsSupport').rules("add", {
        required: function () {
            return ($('#reviewerCommentsSupport').val() == ' ' || $('#reviewerCommentsSupport').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#hrCommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#hrCommentsIFEnvi').val() == ' ' || $('#hrCommentsIFEnvi').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });
    $('#hrCommentsFFSelf').rules("add", {
        required: function () {
            return ($('#hrCommentsFFSelf').val() == ' ' || $('#hrCommentsFFSelf').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });

    $('#hrCommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#hrCommentsFFEnvi').val() == ' ' || $('#hrCommentsFFEnvi').val() == '');
        },
        messages:
        {
            required: "Comment field is required."
        }
    });

    /*hr closure validation*/
    /*end here*/
    $('#anchor_guidelines').click(function () {
        $('#guideLines').dialog({
            autoOpen: false,
            modal: true,
            width: 1000,
            height: 650,
            resizable: false,
            title: "GuideLines"
        });
        $('#guideLines').dialog('open');
    });
    // Set up the jquery grid
    $("#corporateTable").jqGrid({
        // Ajax related configurations
        url: 'http://localhost:25000/ConfirmationProcess/CorporateDetailsLoadGrid',
        postData: { employeeId: empID },
        datatype: "json",
        mtype: "POST",
        // Specify the column names
        colNames: ["Employee ID", "Corporate ID", "Area of contribution" + '\n' + " ( To be filled by Employee) ", "Description of contribution" + '\n' + " ( To be filled by Employee )", "Manager Comments" + '\n' + managerName, "Manager 2 Comments" + '\n' + managerName2, "Reviewer Comments" + '\n' + reviewerName, "HR Reviewer Comments" + '\n' + hrName, ""],
        // Configure the columns
        colModel: [
            { name: "EmployeeID", index: "EmployeeID", hidden: true, width: 100, align: "left" },
            { name: "CorporateId", index: "CorporateId", hidden: true, width: 100, align: "left" },
            { name: "AreaOfContribution", index: "AreaOfContribution", width: 100, align: "left" },
            { name: "ContributionDesc", index: "ContributionDesc", width: 100, align: "left" },
            { name: "ManagerComments", index: "ManagerComments", width: 90, align: "left" },
            { name: "ManagerCommentsSecond", index: "ManagerCommentsSecond", width: 90, align: "left" },
            { name: "ReviewerComments", index: "ReviewerComments", width: 90, align: "left" },
            { name: "HRReviewerComments", index: "ReviewerComments", width: 90, align: "left" },
            {
                name: "Delete",
                index: "Delete",
                width: 12,
                align: "center",
                formatter: function () {
                    if ($('#IsManagerOrEmployee').val() == "Employee" && isClickedViewDetails != "yes") {
                        return '<img src="../../Content/themes/base/images/delete-icon.png" width="15px" height="15px">';
                    } else {
                        return '';
                    }
                }
            }
        ],
        width: 700,
        height: 200,
        // Paging
        toppager: true,
        jsonReader: { repeatitems: false },
        pager: $("#corporateTablePager"),
        rowNum: 5,
        rowList: [5, 10, 20],
        viewrecords: true, // Specify if "total number of records" is displaye d
        height: 'auto',
        autowidth: false,

        caption: "Employee - Confirmation Details",
        gridComplete: function () {
        },
        onCellSelect: function (rowid, iCol) {
            var rowData = $(this).getRowData(rowid);
            var selectedCorporateID = rowData['CorporateId']
            if (iCol == 8 && isClickedViewDetails != "yes") {
                if (isManagerOrEmployee == "Employee") {
                    DeleteCorporateDetail(selectedCorporateID);
                }
            }
            else {
                EditCorporateDetails(rowData);
            }
        }
    }).navGrid("#corporateTablePager",
        { search: false, refresh: false, add: false, edit: false, del: false },
        {},
        {},
        {}
    );
    if ($('#StageID').val() == 4) {
        $("#corporateTable").jqGrid('hideCol', 'ReviewerComments');
        $("#corporateTable").jqGrid('hideCol', 'HRReviewerComments');
    }
    if ($('#IsManagerOrEmployee').val() == 'Employee' && $('#StageID').val() != 7) {
        $('.revViewDetails').hide();
        $('.mngrViewDetails').hide();
        $("#corporateTable").jqGrid('hideCol', 'ManagerCommentsSecond');
        $("#corporateTable").jqGrid('hideCol', 'ManagerComments');
    }
    if (hasManager2 == "noManager2") {  // validation to hide columns if no mngr2 is in the process.
        $('.managerSecond').hide();
        $("#corporateTable").jqGrid('hideCol', 'ManagerCommentsSecond');
    }
    if ($('#IsManagerOrEmployee').val() == 'Employee') {
        $("#corporateTable").jqGrid('hideCol', 'ManagerCommentsSecond');
        $("#corporateTable").jqGrid('hideCol', 'ManagerComments');
        $("#corporateTable").jqGrid('hideCol', 'ReviewerComments');
        $("#corporateTable").jqGrid('hideCol', 'HRReviewerComments');
    }

    $("#btnAddCorporateDetails").click(function () {
        $(".field-validation-error").empty();
        $('input').removeClass("input-validation-error");
        $("#addCorporateDialog #EmployeeID").val($('#EmployeeID').val());
        $("#addCorporateDialog #AreaOfContribution").val('');
        $("#addCorporateDialog #ContributionDesc").val('');
        $("#addCorporateDialog #ManagerComments").val('');
        $("#addCorporateDialog #ManagerCommentsSecond").val('');
        $("#addCorporateDialog #ReviewerComments").val('');
        $("#addCorporateDialog #HRReviewerComments").val('');
        $("#addCorporateDialog #areaOfContribution").val('');
        $("#addCorporateDialog #txtContributionDesc").val('');
        $("#addCorporateDialog #txtManagerComments").val('');
        $("#addCorporateDialog #txtManagerCommentsSecond").val('');
        $("#addCorporateDialog #txtReviewerComments").val('');
        $("#addCorporateDialog #txtHRReviewerComments").val('');
        $('#addCorporateDialog').dialog({
            autoOpen: false,
            modal: true,
            width: 500,
            resizable: false,
            title: "Corporate Contribution Details"
        });
        $('#addCorporateDialog').dialog('open');
    });

    var postUrl = 'SavePerformanceHinderInfo/ConfirmationProcess';
    var postSecondUrl = 'SaveValueDriverInfo/ConfirmationProcess';
    var postThirdUrl = 'SaveGoalAspire/ConfirmationProcess';
    var postForthUrl = 'SaveHrConfirmation/ConfirmationProcess';

    /* PERFORMANCE HINDERS VALIDATION START */
    $('#btnPerfHinderSubmit').submit(function () {
        if ($('#IsManagerOrEmployee').val() == "Employee") {
            if ($('#empCommentsFFSelf').val() == "" || $('#empCommentsFFEnvi').val() == "" || $('#empCommentsIFSelf').val() == "" || $('#empCommentsIFEnvi').val() == "" || $('#empCommentsSupport').val() == "") {
                $('#checkHinders').val(1);
                $("#frmAddPerfHinderDetails").validate().element("#empCommentsFFSelf");
                $("#frmAddPerfHinderDetails").validate().element("#empCommentsIFEnvi");
                $("#frmAddPerfHinderDetails").validate().element("#empCommentsFFEnvi");
                $("#frmAddPerfHinderDetails").validate().element("#empCommentsIFSelf");
                $("#frmAddPerfHinderDetails").validate().element("#empCommentsSupport");
            }
            else {
                $('#checkHinders').val(0);
            }
        }
        else if ($('#IsManagerOrEmployee').val() == "Manager") {
            if ($('#mngrCommentsFFSelf').val() == "" || $('#mngrCommentsFFEnvi').val() == "" || $('#mngrCommentsIFSelf').val() == "" || $('#mngrCommentsIFEnvi').val() == "" || $('#mngrCommentsSupport').val() == "") {
                $('#checkHinders').val(1);
                $("#frmAddPerfHinderDetails").validate().element("#mngrCommentsFFSelf");
                $("#frmAddPerfHinderDetails").validate().element("#mngrCommentsFFEnvi");
                $("#frmAddPerfHinderDetails").validate().element("#mngrCommentsIFSelf");
                $("#frmAddPerfHinderDetails").validate().element("#mngrCommentsIFEnvi");
                $("#frmAddPerfHinderDetails").validate().element("#mngrCommentsSupport");
            }
            else {
                $('#checkHinders').val(0);
            }
        }
        else if ($('#IsManagerOrEmployee').val() == "Manager2") {
            if ($('#mngrCommentsFFSelfSecond').val() == "" || $('#mngrCommentsFFEnviSecond').val() == "" || $('#mngrCommentsIFSelfSecond').val() == "" || $('#mngrCommentsIFEnviSecond').val() == "" || $('#mngrCommentsSupportSecond').val() == "") {
                $('#checkHinders').val(1);
                $("#frmAddPerfHinderDetails").validate().element("#mngrCommentsFFSelfSecond");
                $("#frmAddPerfHinderDetails").validate().element("#mngrCommentsFFEnviSecond");
                $("#frmAddPerfHinderDetails").validate().element("#mngrCommentsIFSelfSecond");
                $("#frmAddPerfHinderDetails").validate().element("#mngrCommentsIFEnviSecond");
                $("#frmAddPerfHinderDetails").validate().element("#mngrCommentsSupportSecond");
            }
            else {
                $('#checkHinders').val(0);
            }
        }
        else if ($('#IsManagerOrEmployee').val() == "Reviewer") {
            if ($('#reviewerCommentsFFEnvi').val() == " " || $('#reviewerCommentsIFSelf').val() == " " || $('#reviewerCommentsIFEnvi').val() == "" || $('#reviewerCommentsFFSelf').val() == "" || $('#reviewerCommentsSupport').val() == "") {
                $('#checkHinders').val(1);
                $("#frmAddPerfHinderDetails").validate().element("#reviewerCommentsFFEnvi");
                $("#frmAddPerfHinderDetails").validate().element("#reviewerCommentsIFSelf");
                $("#frmAddPerfHinderDetails").validate().element("#reviewerCommentsIFEnvi");
                $("#frmAddPerfHinderDetails").validate().element("#reviewerCommentsFFSelf");
                $("#frmAddPerfHinderDetails").validate().element("#reviewerCommentsSupport");
            }
            else {
                $('#checkHinders').val(0);
            }
        }
        else if ($('#IsManagerOrEmployee').val() == "HR" && $('#StageID').val() == 5) {
            if ($('#hrCommentsFFSelf').val() == " " || $('#hrCommentsFFEnvi').val() == " " || $('#hrCommentsIFSelf').val() == "" || $('#hrCommentsIFEnvi').val() == "" || $('#hrCommentsSupport').val() == "") {
                $('#checkHinders').val(1);
                $("#frmAddPerfHinderDetails").validate().element("#hrCommentsFFSelf");
                $("#frmAddPerfHinderDetails").validate().element("#hrCommentsFFEnvi");
                $("#frmAddPerfHinderDetails").validate().element("#hrCommentsIFSelf");
                $("#frmAddPerfHinderDetails").validate().element("#hrCommentsIFEnvi");
                $("#frmAddPerfHinderDetails").validate().element("#hrCommentsSupport");
            }
            else {
                $('#checkHinders').val(0);
            }
        }
    });
    /* PERFORMANCE HINDERS VALIDATION END */

    /*GOALS VALIDATION STARTS*/
    $('#btnSubmit').submit(function () {
        if ($('#ShortTerm').val() == "" || $('#LongTerm').val() == "" || $('#SkillDevPrgm').val() == "") {
            $('#check').val(1);
            $("#frmGoalAquire").validate().element("#ShortTerm");
            $("#frmGoalAquire").validate().element("#LongTerm");
            $("#frmGoalAquire").validate().element("#SkillDevPrgm");
        }
        else {
            $('#check').val(0);
        }
    });
    /*GOALS VALIDATION END*/

    /*HR CLOSURE VALIDATION STARTS*/
    $('#btnSubmitConfirmationDetails').submit(function () {
        var radioName = $('input[name=HRapprove]:checked').val();
        if (radioName == "accept") {
            $("#gradeName").removeData("previousValue");
            //if ($('#empType').val() == "" || $('#empStatus').val() == "" || $('#gradeName').val() == "" || $('#roleName').val() == "") {
            if ($('#empStatus').val() == "" || $('#gradeName').val() == "" || $('#roleName').val() == "") {
                $('#checkHrConf').val(1);
                //$("#frmConfirmationDetails").validate().element("#empType");
                $("#frmConfirmationDetails").validate().element("#empStatus");
                $("#frmConfirmationDetails").validate().element("#gradeName");
                $("#frmConfirmationDetails").validate().element("#roleName");
                $("#frmConfirmationDetails").validate().element("#ConfirmationComments");
            }
            else {
                $('#checkHrConf').val(0);
            }
        }
        else if (radioName == "extend") {
            if ($('#ProbationComments').val() == "") {
                $('#checkHrConf').val(1);
                $("#frmConfirmationDetails").validate().element("#ProbationComments");
            }
            else {
                $('#checkHrConf').val(0);
            }
        }
        else if (radioName == "termintate") {
            if ($('#PIPComments').val() == "") {
                $('#checkHrConf').val(1);
                $("#frmConfirmationDetails").validate().element("#PIPComments");
            }
            else {
                $('#checkHrConf').val(0);
            }
        }
    });
    /*HR CLOSURE VALIDATION END*/

    $('#btnSave').click(function () {
        var apprReject = $('input[name="approve"]:checked').val();
        SaveDetails(apprReject);
    });
    function SaveDetails(apprReject) {
        $(".field-validation-error").empty();
        $('input').removeClass("input-validation-error");
        if (apprReject == 'Approved') {
            /*VALUE DRIVER VALIDATION START*/
            var vaidationmessaggeRating = true;
            var vaidationmessaggeComment = true;
            var gridCount = true;
            var validationArray = [];
            $('#frmValueDriver').find('tr').each(function () {
                if ($(this).find('.ratingInput').length) {
                    var ratingelement = $(this).find('.ratingInput').not(':disabled').not(':hidden').not('.paramDescription');
                    var commentelement = $(this).find('.commentTxtBox').not(':disabled').not(':hidden').not('.paramDescription');
                    var ratingid = ratingelement.attr("id");
                    var commentid = commentelement.attr("id");
                    var ratingVal = parseInt(ratingelement.val());
                    if (ratingVal < minRating || ratingVal > maxRating) {
                        var spanid = "Span_" + ratingid;
                        $("#" + spanid).css("display", "block");
                        vaidationmessaggeRating = false;
                        validationArray.push(false);
                    }
                    else {
                        var spanid = "Span_" + ratingid;
                        $("#" + spanid).css("display", "none");
                        vaidationmessaggeRating = true;
                        validationArray.push(true);
                    }

                    if (commentelement.val() == 0 || commentelement.val() == '0') {
                        var labelid = "Label_" + commentid;
                        $("#" + labelid).css("display", "block");
                        vaidationmessaggeComment = false;
                        validationArray.push(false);
                    }
                    else {
                        var labelid = "Label_" + commentid;
                        $("#" + labelid).css("display", "none");
                        validationArray.push(true);
                        vaidationmessaggeComment = true;
                    }
                }
                else if ($(this).find('.footer-rating-class').not(':disabled').not(':hidden').length) {
                    ratingelement = $(this).find('.footer-rating-class').not(':disabled').not(':hidden');
                    commentelement = $(this).find('.footer-comment-TxtBox').not(':disabled').not(':hidden');
                    ratingVal = parseInt(ratingelement.val());

                    ratingid = ratingelement.attr("id");
                    if (ratingVal < minRating || ratingVal > maxRating) {
                        spanid = "Span_" + ratingid;
                        $("#" + spanid).css("display", "block");
                        vaidationmessaggeRating = false;
                        validationArray.push(false);
                    } else {
                        spanid = "Span_" + ratingid;
                        $("#" + spanid).css("display", "none");
                        vaidationmessaggeRating = true;
                        validationArray.push(true);
                    }
                    commentid = commentelement.attr("id");
                    if ($.trim(commentelement.val()) == "" || commentelement.val() == '') {
                        labelid = "Label_" + commentid;
                        $("#" + labelid).css("display", "block");
                        vaidationmessaggeComment = false;
                        validationArray.push(false);
                    } else {
                        labelid = "Label_" + commentid;
                        $("#" + labelid).css("display", "none");
                        vaidationmessaggeComment = true;
                        validationArray.push(true);
                    }
                }
            });
            /*VALUE DRIVER VALIDATION END*/

            if ($('#StageID').val() != '6') {
                $('#btnPerfHinderSubmit').submit();   // Performance Hinder Validation.

                /*Corporate Contribution Validation*/
                var localGridData = jQuery("#corporateTable").getRowData();
                var corparteValidationArr = true;
                for (var i = 0; i < localGridData.length; i++) {
                    if ($('#IsManagerOrEmployee').val() == "Manager") {
                        var mangrComments = localGridData[i].ManagerComments;
                        if (mangrComments == "") {
                            corparteValidationArr = false;
                        }
                    }
                    else if ($('#IsManagerOrEmployee').val() == "Manager2") {
                        var mangrCommentsSecond = localGridData[i].ManagerCommentsSecond;
                        if (mangrCommentsSecond == "") {
                            corparteValidationArr = false;
                        }
                    }
                    else if ($('#IsManagerOrEmployee').val() == "Reviewer") {
                        var reviewComments = localGridData[i].ReviewerComments;
                        if (reviewComments == "") {
                            corparteValidationArr = false;
                        }
                    }
                    else if ($('#IsManagerOrEmployee').val() == "HR") {
                        var hrRevComments = localGridData[i].HRReviewerComments;
                        if (hrRevComments == "") {
                            corparteValidationArr = false;
                        }
                    }
                }
                /*End*/
            }
            if ($('#IsManagerOrEmployee').val() == "Employee") {
                //$('#btnPerfHinderSubmit').submit();
                $("#btnSubmit").submit();
            }
            if ($('#IsManagerOrEmployee').val() == "HR") {
                //$('#btnPerfHinderSubmit').submit();
            }
            if ($('#StageID').val() == '6') {
                $('#btnSubmitConfirmationDetails').submit();
            }

            $.each($('#frmValueDriver input, #frmValueDriver select, #frmValueDriver textarea').serializeArray(), function (i, obj) {
                $('<input type="hidden">').prop(obj).appendTo($('#frmAddPerfHinderDetails'));
            });
            if ($('#IsManagerOrEmployee').val() == "Employee") {
                var gridCount = true;
                if ($("#corporateTable").getGridParam("reccount") == 0) {
                    gridCount = false;
                    $("#CorporateRecord").dialog({
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        title: "Corporate Contribution Required",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
                if ($("#AddQualificationTable").getGridParam("reccount") == 0) {
                    gridCount = false;
                    $('#QualificationRecord').dialog({
                        autoOpen: false,
                        modal: true,
                        width: 500,
                        resizable: false,
                        title: "Qualification Required.",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                    $('#QualificationRecord').dialog('open');
                }
                if ($("#projectAchievementsTable").getGridParam("reccount") == 0) {
                    gridCount = false;
                    $('#ProjAchievementRecord').dialog({
                        autoOpen: false,
                        modal: true,
                        width: 500,
                        resizable: false,
                        title: "Project Achievement Required.",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                    $('#ProjAchievementRecord').dialog('open');
                }
                if ($("#SkillAquiredTable").getGridParam("reccount") == 0) {
                    gridCount = false;
                    $('#SkillRecord').dialog({
                        autoOpen: false,
                        modal: true,
                        width: 500,
                        resizable: false,
                        title: "Skill Required.",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                    $('#SkillRecord').dialog('open');
                }
                if ($('#check').val() != 0 || $('#checkHinders').val() != 0 || gridCount == false) {
                    return false;
                }
            }
            if ($('#IsManagerOrEmployee').val() == "HR") {
                if ($('#checkHinders').val() != 0)
                    return false;
            }
            if ($('#checkHrConf').val() != 0) {
                return false;
            }
            if (corparteValidationArr == false) {
                $('#CorporateRecord').text('Please enter comments for corporate contribution');
                $("#CorporateRecord").dialog({
                    resizable: false,
                    height: 140,
                    width: 300,
                    modal: true,
                    title: "Corporate Contribution Required",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });
                return false;
            }
            for (var i = 0; i < validationArray.length; i++) {
                if (validationArray[i] == false) {
                    return false;
                }
            }
        }  // reject if ends here
        DisplayLoadingDialog(); // checked
        if ($('#frmAddPerfHinderDetails').valid()) {
            $.ajax({
                url: postUrl,
                type: 'POST',
                async: false,
                data: $('#frmAddPerfHinderDetails').serialize(),
                success: function (results) {
                    if (results.status == true) {
                        /* kunal code  start*/
                        var myArray = [];
                        var empelement = empID;
                        var isMngrOrEmpElement = $('#IsManagerOrEmployee').val();

                        $('#frmValueDriver').find('tr').each(function () {
                            var thisparent = $(this);

                            if ($(this).find('.ratingInput').length) {
                                var ratingelement = thisparent.find('.ratingInput').not(':disabled').not(':hidden').not('.paramDescription');
                                var commentelement = thisparent.find('.commentTxtBox').not(':disabled').not(':hidden').not('.paramDescription');
                                var descriptionelement = thisparent.find('.paramDescription').val();
                                var competencyelement = thisparent.find('.competency').val();

                                if ($('#IsManagerOrEmployee').val() == "Employee") {
                                    var ConfirmationParameter = {
                                        SelfRating: ratingelement.val(),
                                        EmpComments: commentelement.val(),
                                        competencyID: competencyelement,
                                        employeeID: empelement,
                                        IsManagerOrEmployee: isMngrOrEmpElement
                                    };
                                    myArray.push(ConfirmationParameter);
                                }
                                if ($('#IsManagerOrEmployee').val() == "Manager") {
                                    var ConfirmationParameter = {
                                        ManagerRating1: ratingelement.val(),
                                        MngrComments1: commentelement.val(),
                                        competencyID: competencyelement,
                                        employeeID: empelement,
                                        IsManagerOrEmployee: isMngrOrEmpElement
                                    };
                                    myArray.push(ConfirmationParameter);
                                }
                                if ($('#IsManagerOrEmployee').val() == "Manager2") {
                                    var ConfirmationParameter = {
                                        ManagerRating2: ratingelement.val(),
                                        MngrComments2: commentelement.val(),
                                        competencyID: competencyelement,
                                        employeeID: empelement,
                                        IsManagerOrEmployee: isMngrOrEmpElement
                                    };
                                    myArray.push(ConfirmationParameter);
                                }

                                if ($('#IsManagerOrEmployee').val() == "Reviewer") {
                                    var ConfirmationParameter = {
                                        ReviewerRating: ratingelement.val(),
                                        ReviewerComments: commentelement.val(),
                                        competencyID: competencyelement,
                                        employeeID: empelement,
                                        IsManagerOrEmployee: isMngrOrEmpElement
                                    };
                                    myArray.push(ConfirmationParameter);
                                }

                                if ($('#IsManagerOrEmployee').val() == "HR" && $('#StageID').val() != 6) {
                                    var ConfirmationParameter = {
                                        HRrRating: ratingelement.val(),
                                        HrComments: commentelement.val(),
                                        competencyID: competencyelement,
                                        employeeID: empelement,
                                        IsManagerOrEmployee: isMngrOrEmpElement
                                    };
                                    myArray.push(ConfirmationParameter);
                                }
                            }
                            if ($(this).find('.footer-rating-class').length) {
                                if ($('#IsManagerOrEmployee').val() == "Reviewer") {
                                    var overallratingelement = thisparent.find('.footer-rating-class').not(':disabled').not(':hidden');
                                    var overallcommentelement = thisparent.find('.footer-comment-TxtBox').not(':disabled').not(':hidden');
                                    var ConfirmationParameter = {
                                        OverallReviewRating: overallratingelement.val(),
                                        OverallReviewRatingComments: overallcommentelement.val(),
                                    };
                                    myArray.push(ConfirmationParameter);
                                }
                                if ($('#IsManagerOrEmployee').val() == "HR" && $('#StageID').val() != 6) {
                                    var overallratingelement = thisparent.find('.footer-rating-class').not(':disabled').not(':hidden');
                                    var overallcommentelement = thisparent.find('.footer-comment-TxtBox').not(':disabled').not(':hidden');
                                    var ConfirmationParameter = {
                                        OverallReviewHRRating: overallratingelement.val(),
                                        OverallReviewHRComments: overallcommentelement.val(),
                                    };
                                    myArray.push(ConfirmationParameter);
                                }
                            }
                        });
                        /*kunal code end*/
                        var datasecond = JSON.stringify(myArray);
                        if ($('#frmValueDriver').valid()) {
                            $.ajax({
                                type: 'POST',
                                url: postSecondUrl,
                                data: JSON.stringify(myArray),
                                contentType: "application/json; charset=utf-8",
                                success: function (result) {
                                    if (result.status) {
                                        $('#frmGoalAquire').find('textarea').attr('disabled', false);
                                        $.ajax({
                                            type: 'POST',
                                            url: postThirdUrl,
                                            data: $('#frmGoalAquire').serialize(),
                                            success: function (output) {
                                                if (output.status) {
                                                    $("#loading").dialog("close");
                                                    $("#loading").dialog("destroy");
                                                    if ($('#IsManagerOrEmployee').val() == "HR" && $('#StageID').val() == 6) {
                                                        if ($('#frmConfirmationDetails').valid()) {
                                                            $('#frmGoalAquire').find('textarea').attr('disabled', false);
                                                            $.ajax({
                                                                type: 'POST',
                                                                url: postForthUrl,
                                                                data: $('#frmConfirmationDetails').serialize(),
                                                                success: function (outputConf) {
                                                                    if (outputConf.status) {
                                                                        $("#AddRecordSuccessMessege").dialog({
                                                                            resizable: false,
                                                                            height: 140,
                                                                            width: 300,
                                                                            modal: true,
                                                                            buttons: {
                                                                                Ok: function () {
                                                                                    $(this).dialog("close");
                                                                                }
                                                                            }
                                                                        });
                                                                    }
                                                                    else {
                                                                        $("#AddRecordErrorMessege").dialog({
                                                                            resizable: false,
                                                                            height: 140,
                                                                            width: 300,
                                                                            modal: true,
                                                                            buttons: {
                                                                                Ok: function () {
                                                                                    $(this).dialog("close");
                                                                                }
                                                                            }
                                                                        });
                                                                    }
                                                                }
                                                            });
                                                        }
                                                    }
                                                    else {
                                                        $("#AddRecordSuccessMessege").dialog({
                                                            resizable: false,
                                                            height: 140,
                                                            width: 300,
                                                            modal: true,
                                                            buttons: {
                                                                Ok: function () {
                                                                    $(this).dialog("close");
                                                                }
                                                            }
                                                        });
                                                    }
                                                }
                                                else {
                                                    $("#loading").dialog("close");
                                                    $("#loading").dialog("destroy");
                                                    $("#AddRecordErrorMessege").dialog({
                                                        resizable: false,
                                                        height: 140,
                                                        width: 300,
                                                        modal: true,
                                                        buttons: {
                                                            Ok: function () {
                                                                $(this).dialog("close");
                                                            }
                                                        }
                                                    });
                                                }
                                            }
                                        });
                                        if ($('#IsManagerOrEmployee').val() == "Employee") {
                                            $('#frmGoalAquire').find('input,select,textarea').attr('disabled', false);
                                        }
                                        else {
                                            $('#frmGoalAquire').find('input,select,textarea').attr('disabled', true);
                                        }
                                        //                            }
                                    }

                                    else {
                                        $("#loading").dialog("close");
                                        $("#loading").dialog("destroy");
                                        $("#AddPerformanceErrorMessege").dialog({
                                            resizable: false,
                                            height: 140,
                                            width: 300,
                                            modal: true,
                                            buttons: {
                                                Ok: function () {
                                                    $(this).dialog("close");
                                                }
                                            }
                                        });
                                    }
                                }
                            });
                        }
                    }
                    else {
                        $("#AddPerformanceErrorMessege").dialog({
                            resizable: false,
                            height: 140,
                            width: 300,
                            modal: true,
                            buttons: {
                                Ok: function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                    }
                }
            });
        }
        return true;
    };
    $.extend({
        toDictionary: function (query) {
            var parms = {};
            var items = query.split("&"); // split
            for (var i = 0; i < items.length; i++) {
                var values = items[i].split("=");
                var key = decodeURIComponent(values.shift());
                var value = values.join("=")
                parms[key] = decodeURIComponent(value);
            }
            return (parms);
        }
    });

    $('#btnApprove').click(function () {
        var apprReject = $('input[name="approve"]:checked').val();
        var statusReturn = SaveDetails(apprReject);
        if (statusReturn == false)
            return false;

        var empID = $('#EmployeeID').val();
        var status = $('input[name="approve"]:checked').val();
        var isExtendAccept = ""
        if ($('#IsManagerOrEmployee').val() == "HR" && $('#StageID').val() == 6)
            isExtendAccept = $('input[name="HRapprove"]:checked').val();
        var postThirdUrl = 'ApproveConfirmation/ConfirmationProcess'
        $.ajax({
            type: 'POST',
            url: postThirdUrl,
            async: false,
            data: { employeeId: empID, approveReject: status },
            success: function (resultApprove) {
                if (resultApprove.status == true) {
                    $("#ConfirmationRecord").dialog({
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                                $('#btnApprove').attr('disabled');
                                $('#btnSave').hide();
                                $('.button').hide();
                                $('#frmGoalAquire').find('textarea').attr('disabled', true);
                                $('#frmAddPerfHinderDetails').find('input,tr,textarea').attr('disabled', true);
                                $('#frmValueDriver').find('input,tr,textarea').attr('disabled', true);
                                $('#addCorporateContributionDetails').find('input,textarea').attr('disabled', true);
                                $('#addAddQualificationDetails').find('input,select,textarea').attr('disabled', true);
                                $('#frmProjectAchievementDetails').find('input,textarea').attr('disabled', true);
                                /* added below code to hide delete column so that user cant click after the form is approved*/
                                var colPos1 = 8;
                                var corpoarteGrid = $('#corporateTable');
                                corpoarteGrid.jqGrid('hideCol', corpoarteGrid.getGridParam("colModel")[colPos1].name);
                                var colPos2 = 6;
                                var skillGrid = $('#SkillAquiredTable');
                                skillGrid.jqGrid('hideCol', skillGrid.getGridParam("colModel")[colPos2].name);
                                var colPos3 = 9;
                                var qualificationGrid = $('#AddQualificationTable');
                                qualificationGrid.jqGrid('hideCol', qualificationGrid.getGridParam("colModel")[colPos3].name);
                                var colPos4 = 8;
                                var projGrid = $('#projectAchievementsTable');
                                projGrid.jqGrid('hideCol', projGrid.getGridParam("colModel")[colPos4].name);
                                /*start*/

                                var mailurl = 'GetEmailTemplate/ConfirmationProcess';
                                $.ajax({
                                    url: mailurl,
                                    type: 'GET',
                                    data: { employeeId: empID, IsApproveOrReject: status.toString(), IsApproveOrReject: status.toString(), IsAcceptExtendPIP: isExtendAccept.toString() },
                                    success: function (data) {
                                        $('#MailTemplateDialog').html(data);
                                        $("#MailTemplateDialog").dialog({
                                            resizable: false,
                                            height: 520,
                                            width: 800,
                                            modal: true,
                                            title: 'Send Mail'
                                        });

                                        $.validator.unobtrusive.parse($("#addMailTemplate"));

                                        //$('#sendInitiateMail').click(function () {
                                        //    $("#CCErrorMessage").hide();
                                        //    $("#ToErrorMessage").hide();
                                        //    if ($('#addMailTemplate').valid()) {
                                        //         DisplayLoadingDialog(); // checked

                                        //        var SendMailUrl = 'SendEmail/ConfirmationProcess';
                                        //        $.ajax({
                                        //            url: SendMailUrl,
                                        //            type: 'POST',
                                        //            data: $('#addMailTemplate').serialize(),
                                        //            success: function (data) {
                                        //                $("#loading").dialog("close");
                                        //                $("#loading").dialog("destroy");
                                        //                if (data.validCcId == true && data.validtoId == true) {
                                        //                    if (data.status == true) {
                                        //                        $("#MailTemplateDialog").dialog('destroy');
                                        //                        $("#mailSuccessMessage").dialog({
                                        //                            closeOnEscape: false,
                                        //                            resizable: false,
                                        //                            height: 140,
                                        //                            width: 300,
                                        //                            modal: true,
                                        //                            title: 'mail Process',
                                        //                            buttons: {
                                        //                                Ok: function () {
                                        //                                    $(this).dialog("close");
                                        //                                }
                                        //                            }
                                        //                        });
                                        //                    }
                                        //                    else if (data.status == false) {
                                        //                    $("#MailIDError").dialog({
                                        //                        title: 'Mail Error',
                                        //                        resizable: false,
                                        //                        height: 'auto',
                                        //                        width: 'auto',
                                        //                        modal: true,
                                        //                        buttons: {
                                        //                            Ok: function () {
                                        //                                $(this).dialog("close");
                                        //                            }
                                        //                        },
                                        //                        close: function () {
                                        //                            $(this).dialog("destroy");
                                        //                        }
                                        //                    }); //end dialog
                                        //                   }
                                        //                }
                                        //                else if (data.status == "Error") {
                                        //                    $("#errorDialog").dialog({
                                        //                        title: 'Mail Error',
                                        //                        resizable: false,
                                        //                        height: 'auto',
                                        //                        width: 'auto',
                                        //                        modal: true,
                                        //                        buttons: {
                                        //                            Ok: function () {
                                        //                                $(this).dialog("close");
                                        //                            }
                                        //                        }
                                        //                    }); //end dialog
                                        //                }
                                        //                else {
                                        //                    if (data.validCcId == false)
                                        //                        $("#CCErrorMessage").show();
                                        //                    if (data.validtoId == false)
                                        //                        $("#ToErrorMessage").show();
                                        //                    return false;
                                        //                }
                                        //            },
                                        //            error: function () {
                                        //                $("#loading").dialog("close");
                                        //                $("#loading").dialog("destroy");
                                        //                $("#errorDialog").dialog({
                                        //                    title: 'Mail Error',
                                        //                    resizable: false,
                                        //                    height: 'auto',
                                        //                    width: 'auto',
                                        //                    modal: true,
                                        //                    buttons: {
                                        //                        Ok: function () {
                                        //                            $(this).dialog("close");
                                        //                        }
                                        //                    }
                                        //                }); //end dialog
                                        //            }
                                        //        });
                                        //    }
                                        //});
                                    }
                                });
                                /*end*/
                            }
                        }
                    });
                }
                else if (resultApprove.status == "Error") {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    $("#errorDialog").dialog({
                        title: 'Confirmation Process',
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    }); //end dialog
                }
                else {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    $("#AddPerformanceErrorMessege").dialog({
                        title: 'Confirmation Process',
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            }
        });
        //}
    });

    function EditCorporateDetails(Object) {
        $(".field-validation-error").empty();
        $('input').removeClass("input-validation-error");
        $("#addCorporateDialog #EmployeeID").val($('#EmployeeID').val());
        $('#addCorporateDialog #CorporateId').val(Object['CorporateId']);
        $('#addCorporateDialog #AreaOfContribution').val(Object['AreaOfContribution']);
        $('#addCorporateDialog #ContributionDesc').val(Object['ContributionDesc']);
        $('#addCorporateDialog #ManagerComments').val(Object['ManagerComments']);
        $('#addCorporateDialog #ManagerCommentsSecond').val(Object['ManagerCommentsSecond']);
        $('#addCorporateDialog #ReviewerComments').val(Object['ReviewerComments']);
        $('#addCorporateDialog #HRReviewerComments').val(Object['HRReviewerComments']);
        $('#addCorporateDialog #areaOfContribution').val(Object['AreaOfContribution']);
        $('#addCorporateDialog #txtContributionDesc').val(Object['ContributionDesc']);
        $('#addCorporateDialog #txtManagerComments').val(Object['ManagerComments']);
        $('#addCorporateDialog #txtManagerCommentsSecond').val(Object['ManagerCommentsSecond']);
        $('#addCorporateDialog #txtReviewerComments').val(Object['ReviewerComments']);
        $('#addCorporateDialog #txtHRReviewerComments').val(Object['HRReviewerComments']);
        $('#addCorporateDialog').dialog
        (
            {
                autoOpen: false,
                modal: true,
                width: 500,
                resizable: false,
                title: "Edit Corporate Contribution Details"
            }
        );
        $('#addCorporateDialog').dialog('open');
    }

    function DeleteCorporateDetail(selectedCorporateID) {
        $('#DeleteConfirmationDialog').dialog(
            {
                autoOpen: false,
                modal: true,
                width: 300,
                height: 125,
                resizable: false,
                title: "Delete Corporate Contribution Detail",
                buttons:
                {
                    Ok: function () {
                        $.ajax({
                            url: 'DeleteCorporateDetails/ConfirmationProcess',
                            data: { CorporateID: selectedCorporateID },
                            success: function (data) {
                                if (data.status == true) {
                                    $("#DeleteConfirmationDialog").dialog("close");
                                    $("#DeleteConfirmation").dialog(
                                    {
                                        modal: true,
                                        resizable: false,
                                        height: 140,
                                        width: 300,
                                        title: "Deleted",
                                        buttons:
                                        {
                                            "Ok": function () {
                                                $(this).dialog('close');
                                                jQuery("#corporateTable").trigger("reloadGrid");
                                            }
                                        }
                                    }
                                );
                                }
                                else if (data.status == "Error") {
                                    $("#errorDialog").dialog({
                                        title: 'Confirmation Process',
                                        resizable: false,
                                        height: 'auto',
                                        width: 'auto',
                                        modal: true,
                                        buttons: {
                                            Ok: function () {
                                                $(this).dialog("close");
                                            }
                                        }
                                    }); //end dialog
                                }
                                else {
                                    $("#errorDialog").dialog({
                                        title: 'Confirmation Process',
                                        resizable: false,
                                        height: 'auto',
                                        width: 'auto',
                                        modal: true,
                                        buttons: {
                                            Ok: function () {
                                                $(this).dialog("close");
                                            }
                                        }
                                    }); //end dialog
                                }
                            } //end success
                        });
                    },
                    Cancel: function () {
                        $(this).dialog('close');
                    }
                }
            });
        $('#DeleteConfirmationDialog').dialog('open');
    }
});