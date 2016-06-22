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
    $.ajaxSetup({ async: false }, { cache: false });
    function validate() {
        var validationArray = [];
        var rows = $("#ParmList_table tr:gt(0)");
        rows.each(function (i, el) {
            var $tds = $(this).find('td');
            var Id = $tds.eq(0).html();
            var parmId = $(Id).attr("value");
            var two = $tds.eq(1).html();
            var spanid = "span_Reviewer1Rating_" + parmId;
            if (UserRole == 'Reviewer1') {
                //                var commentsID = "Comments1_" + parmId;
                //                var rev1Comment = $("#" + commentsID).val();

                var RatingsID = "Ratings1_" + parmId;
                var rev1Rating = $("#" + RatingsID).val();
                if (ratingVal < minRating || ratingVal > maxRating) {
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
            }
        });
        for (var i = 0; i < validationArray.length; i++) {
            if (validationArray[i] == false) {
                return false;
            }
        }
    }

    if ($('#IsViewDetails').val() == "Yes" && $('#LinkClicked').val() == "GroupHead" && $('#IsManagerOrEmployee').val() == "GroupHead" && ($('#StageID').val() == 7 || $('#StageID').val() == 8 || $('#IsIDFFrozen').val() == "True") && $('#IsUnfreezedByAdmin').val() == "True") {
        $('#btnSaveAppraisal').show();
        $('#OverallGrpHeadRating').attr('disabled', false);
        $('#OverallGrpHeadComments').attr('disabled', false);
    }

    if ($('#IsViewDetails').val() == "Yes" && $('#LinkClicked').val() == "AppraisalCoordinator" && ($('#StageID').val() == 7 || $('#StageID').val() == 8 || $('#IsIDFFrozen').val() == "True")) {
        $("#btnViewGHHistory").show();
    }

    if (($('#IsManagerOrEmployee').val() == 'Appraiser1' || $('#IsManagerOrEmployee').val() == 'Appraiser2') && ($('#StageID').val() >= 2)) {
        $('.HideForApp').hide();
    }

    if ($('#IsManagerOrEmployee').val() == 'Appraiser1' && $('#StageID').val() >= 1) {
        $('.APP1CantSee').hide();
    }
    if ($('#IsManagerOrEmployee').val() == 'Appraiser2' && $('#StageID').val() >= 1) {
        $('.APP2CantSee').hide();
    }

    if ($('#IsManagerOrEmployee').val() == 'Reviewer1' && $('#StageID').val() >= 1) {
        $('.Rew1Cantsee').hide();
    }

    if ($('#IsManagerOrEmployee').val() == 'Reviewer2' && $('#StageID').val() >= 1) {
        $('.Rew2Cantsee').hide();
    }
    if (($('#IsManagerOrEmployee').val() == 'Reviewer2') && ($('#StageID').val() >= 2)) {
        $('.HideForRew2').hide();
    }
    if ($('#IsManagerOrEmployee').val() == 'Appraiser2' && $('#StageID').val() >= 2) {
        $('.Hide2App').hide();
    }
    if ($('#IsManagerOrEmployee').val() == 'Appraiser1' && $('#StageID').val() >= 2) {
        $('.Hide1App').hide();
    }
    if ($('#IsManagerOrEmployee').val() == 'Appraiser1' && $('#StageID').val() == 4) {
        $('.App1IDF').attr("disabled", true);
    }
    if (($('#IsManagerOrEmployee').val() == "Appraiser1" || $('#IsManagerOrEmployee').val() == "Appraiser2") && $('#StageID').val() == 1 && $('#IsViewDetails').val() != "Yes")
        $('#btnRejectAppraisal').show();
    else
        $('#btnRejectAppraisal').hide();

    if ($('#IsManagerOrEmployee').val() == 'Employee' && $('#IsViewDetails').val() != "Yes") {
        $("#frmGoalAquireAppraisal").validate();
    }
    if (($('#IsManagerOrEmployee').val() == "Reviewer1" || $('#IsManagerOrEmployee').val() == "Reviewer2" || $('#IsManagerOrEmployee').val() == "GroupHead") && ($('#IsViewDetails').val() != "Yes")) {
        $("#NextAppraisal").hide();
    }

    if ($('#IsManagerOrEmployee').val() == 'Employee' || (($('#IsManagerOrEmployee').val() == "Appraiser1" || $('#IsManagerOrEmployee').val() == "Appraiser2") && $('#StageID').val() < 3)) {
        $("#NextAppraisal").hide();
    }

    if ($('#IsManagerOrEmployee').val() == "Appraiser1" && $('#IsViewDetails').val() == "Yes" && $('#StageID').val() >= 5)
        $("#NextAppraisal").show();

    if ($('#IsManagerOrEmployee').val() == "Appraiser2")
        $("#NextAppraisal").hide();

    if ($('#StageID').val() == 7 || $('#StageID').val() == 8 || ($('#FromStageID').val() == 5 && $('#ToStageID').val() == 4) && $('#IsManagerOrEmployee').val() == 'Employee' && $('#IsViewDetails').val() == "Yes")
        $("#NextAppraisal").show();

    if ($('#IsViewDetails').val() == "Yes") {
        $("#btnAddProjectAchievementsAppraisalDetails").hide();
        $('.containerbtn').hide();
        $("#btnAddCorporateDetailsAppraisal").hide();
        $("#btnAddSkillAquiredAppraisalDetails").hide();
        $("#btnAddQualificationAppraisalDetails").hide();
    }
    if ($('#IsPerformanceYearFrozen').val() == "True" && ($('#IsManagerOrEmployee').val() == 'Employee' || $('#IsManagerOrEmployee').val() == "Appraiser1" || $('#IsManagerOrEmployee').val() == "Appraiser2" ||
            $('#IsManagerOrEmployee').val() == "Reviewer1" || $('#IsManagerOrEmployee').val() == "Reviewer2") && $('#StageID').val() <= 3) {
        $("#btnAddProjectAchievementsAppraisalDetails").hide();
        $('.containerbtn').hide();
        $("#btnAddCorporateDetailsAppraisal").hide();
        $("#btnAddSkillAquiredAppraisalDetails").hide();
        $("#btnAddQualificationAppraisalDetails").hide();
        $("#performanceHinderDetailRecords #frmAddPerfHinderAppraisalDetails").find('input, textarea, button, select').attr("disabled", "disabled");
        $("#frmGoalAquireAppraisal").find('input, textarea, button, select').attr("disabled", "disabled");
        $("#frmAppraisalParameter").find('input, tr, textarea, dropdown').attr("disabled", true);
        $("#btnSaveAppraisal").hide();
        $('#btnApprove').hide();
        $('#btnRejectAppraisal').hide();
        $('#btnRejectAppraisalReviewer').hide();
        if ($('#IsManagerOrEmployee').val() == "Reviewer1" || $('#IsManagerOrEmployee').val() == "Reviewer2") {
            $('#FreezePerformanceAppraisalNext').show();
            $("#NextAppraisal").hide();
        }
    }

    if ($('#NoAppraiser2').val() == "noAppraiser2") { // validation to hide columns if no Appr2 is in the process.
        $('.managerSecond').hide();
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Appraiser2Comments');
    }

    if ($('#NoReviewer2').val() == "noReviewer2") { // validation to hide columns if no Rev2 is in the process.
        $('.reviewerSecond').hide();
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Reviewer2Comments');
    }
    if ($('#IsManagerOrEmployee').val() == 'Employee') {
        $('.revViewDetails').hide();
        $('.mngrViewDetails').hide();
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Appraiser1Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Appraiser2Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Reviewer1Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Reviewer2Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'GrpHeadComments');
    }
    if ($('#IsManagerOrEmployee').val() == 'Appraiser2') {
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Appraiser1Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Reviewer1Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Reviewer2Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'GrpHeadComments');
    }
    if ($('#IsManagerOrEmployee').val() == 'Appraiser1') {
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Appraiser2Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Reviewer1Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Reviewer2Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'GrpHeadComments');
    }

    if ($('#IsManagerOrEmployee').val() == "Reviewer1") {
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Reviewer2Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'GrpHeadComments');
    }

    if ($('#IsManagerOrEmployee').val() == "Reviewer2") {
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Reviewer1Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'GrpHeadComments');
    }
    if (($('#IsManagerOrEmployee').val() == 'Employee' && $('#LinkClicked').val() == "SelfAppraisal") && ($('#StageID').val() == 7 || $('#StageID').val() == 8) && $('#IsPerformanceYearFrozen').val() == 'True') {
        $('.revViewDetails').show();
        $('.mngrViewDetails').show();
        $("#corporateTableAppraisal").jqGrid('showCol', 'Appraiser1Comments');
        $("#corporateTableAppraisal").jqGrid('showCol', 'Appraiser2Comments');
        $("#corporateTableAppraisal").jqGrid('showCol', 'Reviewer1Comments');
        $("#corporateTableAppraisal").jqGrid('showCol', 'Reviewer2Comments');
        $("#corporateTableAppraisal").jqGrid('showCol', 'GrpHeadComments');
        $('.ratingshide').hide();
        $('#NextAppraisal').show();
    }
    if ((($('#UserRole').val() == 'HR Admin' && $('#LinkClicked').val() == "AppraisalCoordinator") || ($('#UserRole').val() == 'GroupHead' && $('#LinkClicked').val() == "GroupHead"))
    && ($('#StageID').val() == 7 || $('#StageID').val() == 8) && $('#IsPerformanceYearFrozen').val() == 'True') {
        $('.revViewDetails').show();
        $('.mngrViewDetails').show();
        $("#corporateTableAppraisal").jqGrid('showCol', 'Appraiser1Comments');
        $("#corporateTableAppraisal").jqGrid('showCol', 'Appraiser2Comments');
        $("#corporateTableAppraisal").jqGrid('showCol', 'Reviewer1Comments');
        $("#corporateTableAppraisal").jqGrid('showCol', 'Reviewer2Comments');
        $("#corporateTableAppraisal").jqGrid('showCol', 'GrpHeadComments');
        $('#NextAppraisal').show();
    }
    $('#anchor_AppraisalGuidelines').click(function () {
        $('#appraisalGuideLines').dialog({
            autoOpen: false,
            modal: true,
            width: 1000,
            height: 650,
            resizable: false,
            title: "GuideLines"
        });
        $('#appraisalGuideLines').dialog('open');
    });

    if ($('#StageID').val() == 1) {
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Reviewer1Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'Reviewer2Comments');
        $("#corporateTableAppraisal").jqGrid('hideCol', 'GrpHeadComments');
    }

    if ($('#StageID').val() == 2) {
        $("#corporateTableAppraisal").jqGrid('hideCol', 'GrpHeadComments');
    }

    $("#btnAddCorporateDetailsAppraisal").click(function () {
        ISFormSubmitedorNotCorporate();
        $(".field-validation-error").empty();
        $('input').removeClass("input-validation-error");
        $("#addCorporateDialogAppraisal #EmployeeID").val($('#EmployeeID').val());
        $("#addCorporateDialogAppraisal #appraisalId").val($('#appraisalId').val());
        $("#addCorporateDialogAppraisal #AreaOfContribution").val('');
        $("#addCorporateDialogAppraisal #ContributionDesc").val('');
        $("#addCorporateDialogAppraisal #ManagerComments").val('');
        $("#addCorporateDialogAppraisal #ManagerCommentsSecond").val('');
        $("#addCorporateDialogAppraisal #ReviewerComments").val('');
        $("#addCorporateDialogAppraisal #HRReviewerComments").val('');
        $("#addCorporateDialogAppraisal #areaOfContribution").val('');
        $("#addCorporateDialogAppraisal #txtContributionDesc").val('');
        $("#addCorporateDialogAppraisal #txtManagerComments").val('');
        $("#addCorporateDialogAppraisal #txtManagerCommentsSecond").val('');
        $("#addCorporateDialogAppraisal #txtReviewerComments").val('');
        $("#addCorporateDialogAppraisal #txtHRReviewerComments").val('');
        $("#addCorporateDialogAppraisal #CorporateId").val('');
        $('#addCorporateDialogAppraisal').dialog({
            autoOpen: false,
            modal: true,
            width: 500,
            resizable: false,
            title: "Corporate Contribution Details"
        });
        $('#addCorporateDialogAppraisal').dialog('open');
    });
    if ($('#StageID').val() == 0 || $('#StageID').val() == 1) {
        $("h4").removeClass("PrintNewPage");
    }
});

function EditCorporateDetails(Object) {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#addCorporateDialogAppraisal #EmployeeID").val($('#EmployeeID').val());
    $('#addCorporateDialogAppraisal #appraisalId').val($('#appraisalId').val());
    $("#addCorporateDialogAppraisal #AreaOfContribution").val(Object['AreaOfContribution']);
    $('#addCorporateDialogAppraisal #ContributionDesc').val(Object['ContributionDesc']);
    $('#addCorporateDialogAppraisal #Appraiser1Comments').val(Object['Appraiser1Comments']);
    $('#addCorporateDialogAppraisal #Appraiser2Comments').val(Object['Appraiser2Comments']);
    $('#addCorporateDialogAppraisal #Reviewer1Comments').val(Object['Reviewer1Comments']);
    $('#addCorporateDialogAppraisal #Reviewer2Comments').val(Object['Reviewer2Comments']);
    $('#addCorporateDialogAppraisal #GrpHeadComments').val(Object['GrpHeadComments']);

    $('#addCorporateDialogAppraisal #CorporateId').val(Object['CorporateId']);
    $("#addCorporateDialogAppraisal #areaOfContribution").val(Object['AreaOfContribution']);
    $('#addCorporateDialogAppraisal #txtContributionDesc').val(Object['ContributionDesc']);
    $('#addCorporateDialogAppraisal #txtAppraiser1Comments').val(Object['Appraiser1Comments']);
    $('#addCorporateDialogAppraisal #txtAppraiser2Comments').val(Object['Appraiser2Comments']);
    $('#addCorporateDialogAppraisal #txtReviewer1Comments').val(Object['Reviewer1Comments']);
    $('#addCorporateDialogAppraisal #txtReviewer2Comments').val(Object['Reviewer2Comments']);
    $('#addCorporateDialogAppraisal #txtGrpHeadComments').val(Object['GrpHeadComments']);
    if ($('#IsManagerOrEmployee').val() == "Appraiser1" && $('#LinkClicked').val() != "AppraisalCoordinator") {
        $('#addCorporateDialogAppraisal #Appraiser2Comments').hide();
    }
    else if ($('#IsManagerOrEmployee').val() == "Appraiser2" && $('#LinkClicked').val() != "AppraisalCoordinator") {
        $('.managerOne').hide();
        $('#addCorporateDialogAppraisal #Appraiser1Comments').hide();
    }
    else if ($('#IsManagerOrEmployee').val() == "Reviewer1" && $('#LinkClicked').val() != "AppraisalCoordinator") {
        $('.reviewerSecond').hide();
    }
    else if ($('#IsManagerOrEmployee').val() == "Reviewer2" && $('#LinkClicked').val() != "AppraisalCoordinator") {
        $('.reviewerOneHide').hide();
    }
    if (($('#IsManagerOrEmployee').val() == "Appraiser1") && ($('#IsViewDetails').val() != "Yes") && ($('#StageID').val() == 4)) {
        $('#addCorporateDialogAppraisal #Appraiser1Comments').attr('disabled', 'disabled');
    }

    if ($('#IsViewDetails').val() != "Yes") {
        $('#addCorporateDialogAppraisal #Appraiser1Comments').attr('disabled', 'disabled');
    }
    if (($('#IsManagerOrEmployee').val() == "Appraiser1") && ($('#StageID').val() == 1) && $('#IsViewDetails').val() != "Yes") {
        $('#addCorporateDialogAppraisal #Appraiser1Comments').removeAttr('disabled');
    }

    if ($('#IsViewDetails').val() == "Yes" && $('#LinkClicked').val() == "Appraiser") {
        if ($('#FromStageID').val() == 1 && $('#ToStageID').val() == 0) {
            $('.managerOne').hide();
            $('.managerSecond').hide();
            $('.btnCorporate').hide();
            $('.btnReset').hide();
        }
    }

    if ($('#IsViewDetails').val() == "Yes" && $('#LinkClicked').val() == "AppraisalCoordinator") {
        if (ApproverId == Emp1 && $('#FromStageID').val() == 0 && $('#ToStageID').val() == 1) {
            $('.managerOne').hide();
            $('.managerSecond').hide();
            $('.reviewerOneHide').hide();
            $('.reviewerSecond').hide();
        }
        if (ApproverId == App1 && (($('#FromStageID').val() == 1 && $('#ToStageID').val() == 1) || ($('#FromStageID').val() == 1 && $('#ToStageID').val() == 2) || ($('#FromStageID').val() == 1 && $('#ToStageID').val() == 0))) {
            $('.managerOne').show();
            $('.managerSecond').hide();
            $('.reviewerOneHide').hide();
            $('.reviewerSecond').hide();
        }
        if (ApproverId == App2 && (($('#FromStageID').val() == 1 && $('#ToStageID').val() == 2) || ($('#FromStageID').val() == 1 && $('#ToStageID').val() == 1) || ($('#FromStageID').val() == 1 && $('#ToStageID').val() == 0))) {
            $('.managerOne').show();
            $('.managerSecond').show();
            $('.reviewerOneHide').hide();
            $('.reviewerSecond').hide();
        }
        if (ApproverId == Rew1 && (($('#FromStageID').val() == 2 && $('#ToStageID').val() == 2) || ($('#FromStageID').val() == 2 && $('#ToStageID').val() == 3) || ($('#FromStageID').val() == 2 && $('#ToStageID').val() == 1))) {
            $('.managerOne').show();
            $('.managerSecond').show();
            $('.reviewerOneHide').show();
            $('.reviewerSecond').hide();
        }
        if (ApproverId == Rew2 && (($('#FromStageID').val() == 2 && $('#ToStageID').val() == 2) || ($('#FromStageID').val() == 2 && $('#ToStageID').val() == 3) || ($('#FromStageID').val() == 2 && $('#ToStageID').val() == 1))) {
            $('.managerOne').show();
            $('.managerSecond').show();
            $('.reviewerOneHide').show();
            $('.reviewerSecond').show();
        }

        if ($('#StageID').val() == 3) {
            $('.managerOne').show();
            $('.managerSecond').show();
            $('.reviewerOneHide').show();
            $('.reviewerSecond').show();
        }
        if ($('#StageID').val() >= 4) {
            $('.managerOne').show();
            $('.managerSecond').show();
            $('.reviewerOneHide').show();
            $('.reviewerSecond').show();
        }

        if ($('#NoReviewer2').val() == "noAppraiser2") {
            $('.managerSecond').hide();
        }

        if ($('#NoAppraiser2').val() == "noReviewer2") {
            $('.reviewerSecond').hide();
        }
    }
    $('#addCorporateDialogAppraisal').dialog
            (
                {
                    autoOpen: false,
                    modal: true,
                    width: 560,
                    resizable: false,
                    title: "Edit Corporate Contribution Details"
                }
            );
    $('#addCorporateDialogAppraisal').dialog('open');
}

function DeleteCorporateDetail(selectedCorporateID) {
    $('#DeleteConfirmationDialog').dialog(
            {
                autoOpen: false,
                modal: true,
                width: 300,
                height: 'auto',
                dialogClass: "noclose",
                resizable: false,
                title: "Delete Corporate Contribution Detail",
                buttons:
                {
                    Ok: function () {
                        $.ajax({
                            url: 'DeleteCorporateDetailsAppraisal/Appraisal',
                            data: { corporateId: selectedCorporateID },
                            success: function (data) {
                                if (data.status == true) {
                                    $("#DeleteConfirmationDialog").dialog("close");
                                    $("#DeleteConfirmation").dialog(
                                        {
                                            modal: true,
                                            resizable: false,
                                            height: 'auto',
                                            width: 300,
                                            title: "Deleted",
                                            dialogClass: "noclose",
                                            buttons:
                                            {
                                                "Ok": function () {
                                                    $(this).dialog('close');
                                                    jQuery("#corporateTableAppraisal").trigger("reloadGrid");
                                                }
                                            }
                                        }
                                    );
                                } else if (data.status == "Error") {
                                    $("#errorDialog").dialog({
                                        title: 'Appraisal Process',
                                        resizable: false,
                                        height: 'auto',
                                        width: 'auto',
                                        modal: true,
                                        dialogClass: "noclose",
                                        buttons: {
                                            Ok: function () {
                                                $(this).dialog("close");
                                            }
                                        }
                                    }); //end dialog
                                } else {
                                    $("#errorDialog").dialog({
                                        title: 'Appraisal Process',
                                        resizable: false,
                                        height: 'auto',
                                        width: 'auto',
                                        modal: true,
                                        dialogClass: "noclose",
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

/* PERFORMANCE HINDERS VALIDATION START */
$('#btnPerfHinderSubmit').submit(function () {
    if ($('#IsManagerOrEmployee').val() == "Employee") {
        if ($('#empCommentsFFSelf').val() == "" || $('#empCommentsFFEnvi').val() == "" || $('#empCommentsIFSelf').val() == "" || $('#empCommentsIFEnvi').val() == "" || $('#empCommentsSupport').val() == "") {
            $('#checkHinders').val(1);
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#empCommentsFFSelf");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#empCommentsIFEnvi");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#empCommentsFFEnvi");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#empCommentsIFSelf");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#empCommentsSupport");
        } else {
            $('#checkHinders').val(0);
        }
    } else if ($('#IsManagerOrEmployee').val() == "Appraiser1") {
        if ($('#Appraiser1CommentsFFSelf').val() == "" || $('#Appraiser1CommentsFFEnvi').val() == "" || $('#Appraiser1CommentsIFSelf').val() == "" || $('#Appraiser1CommentsIFEnvi').val() == "" || $('#Appraiser1CommentsSupport').val() == "") {
            $('#checkHinders').val(1);
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Appraiser1CommentsFFSelf");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Appraiser1CommentsFFEnvi");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#mAppraiser1CommentsIFSelf");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Appraiser1CommentsIFEnvi");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Appraiser1CommentsSupport");
        } else {
            $('#checkHinders').val(0);
        }
    } else if ($('#IsManagerOrEmployee').val() == "Appraiser2") {
        if ($('#Appraiser2CommentsFFSelf').val() == "" || $('#Appraiser2CommentsFFEnvi').val() == "" || $('#Appraiser2CommentsIFSelf').val() == "" || $('#Appraiser2CommentsIFEnvi').val() == "" || $('#Appraiser2CommentsSupport').val() == "") {
            $('#checkHinders').val(1);
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Appraiser2CommentsFFSelf");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Appraiser2CommentsFFEnvi");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Appraiser2CommentsIFSelf");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Appraiser2CommentsIFEnvi");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Appraiser2CommentsSupport");
        } else {
            $('#checkHinders').val(0);
        }
    } else if ($('#IsManagerOrEmployee').val() == "Reviewer1") {
        if ($('#Reviewer1CommentsFFEnvi').val() == " " || $('#Reviewer1CommentsIFSelf').val() == " " || $('#Reviewer1CommentsIFEnvi').val() == "" || $('#Reviewer1CommentsFFSelf').val() == "" || $('#Reviewer1CommentsSupport').val() == "") {
            $('#checkHinders').val(1);
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Reviewer1CommentsFFEnvi");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Reviewer1CommentsIFSelf");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Reviewer1CommentsIFEnvi");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Reviewer1CommentsFFSelf");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Reviewer1CommentsSupport");
        } else {
            $('#checkHinders').val(0);
        }
    } else if ($('#IsManagerOrEmployee').val() == "Reviewer2") {
        if ($('#Reviewer2CommentsFFSelf').val() == " " || $('#Reviewer2CommentsFFEnvi').val() == " " || $('#Reviewer2CommentsIFSelf').val() == "" || $('#Reviewer2CommentsIFEnvi').val() == "" || $('#Reviewer2CommentsSupport').val() == "") {
            $('#checkHinders').val(1);
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Reviewer2CommentsFFSelf");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Reviewer2CommentsFFEnvi");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Reviewer2CommentsIFSelf");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Reviewer2CommentsIFEnvi");
            $("#frmAddPerfHinderAppraisalDetails").validate().element("#Reviewer2CommentsSupport");
        } else {
            $('#checkHinders').val(0);
        }
    }
});
/* PERFORMANCE HINDERS VALIDATION END */

/*GOALS VALIDATION STARTS*/
$('#btnSubmit').submit(function () {
    if ($('#ShortTerm').val() == "" || $('#LongTerm').val() == "" || $('#SkillDevPrgm').val() == "") {
        $('#check').val(1);
        $("#frmGoalAquireAppraisal").validate().element("#ShortTerm");
        $("#frmGoalAquireAppraisal").validate().element("#LongTerm");
        $("#frmGoalAquireAppraisal").validate().element("#SkillDevPrgm");
    } else {
        $('#check').val(0);
    }
});
/*GOALS VALIDATION END*/

$('#anchor_guidelinesAppraisalRating').click(function () {
    $('#guideLinesAppraisalRating').dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 510,
        resizable: false,
        title: "Guidelines"
    });
    $('#guideLinesAppraisalRating').dialog('open');
});

if ($('#IsViewDetails').val() == "Yes" || $('#StageID').val() == 1) {
    $("#NextAppraisal").hide();
}
if ($('#IsManagerOrEmployee').val() == "Employee") {
    /* below id's r for perfHinder*/
    $('#empCommentsFFSelf').removeAttr('disabled');
    $('#empCommentsFFEnvi').removeAttr('disabled');
    $('#empCommentsIFSelf').removeAttr('disabled');
    $('#empCommentsIFEnvi').removeAttr('disabled');
    $('#empCommentsSupport').removeAttr('disabled');
    if ($('#IsViewDetails').val() == "Yes") {
        $('#empCommentsFFSelf').attr('disabled', true);
        $('#empCommentsFFEnvi').attr('disabled', true);
        $('#empCommentsIFSelf').attr('disabled', true);
        $('#empCommentsIFEnvi').attr('disabled', true);
        $('#empCommentsSupport').attr('disabled', true);
        $('.btnReset').hide();
        $('.btnCorporate').hide();
    }
    $('#tdReviewer1CommentsFFSelf').show();
    $('#tdReviewer1CommentsFFEnvi').show();
    $('#tdReviewer1CommentsIFSelf').show();
    $('#tdReviewer1CommentsIFEnvi').show();
    $('#tdReviewer1CommentsSupport').show();
    $('#tdReviewer2CommentsFFSelf').show();
    $('#tdReviewer2CommentsFFEnvi').show();
    $('#tdReviewer2CommentsIFSelf').show();
    $('#tdReviewer2CommentsIFEnvi').show();
    $('#tdReviewer2CommentsSupport').show();
    $('#thReview2Comments').show();
}
if ($('#IsManagerOrEmployee').val() == "Appraiser1") {
    $('#btnAddSkillAquiredAppraisalDetails').hide();
    $('#btnAddProjectAchievementsAppraisalDetails').hide();
    $('.containerbtn').hide();
    $('.managerSecond ').hide();
    $('.appraiserTwoHiderating').hide();
    $('.appraiserTwoHideComment').hide();
    $('#btnAddCorporateDetailsAppraisal').hide();
    $('#btnAddQualificationAppraisalDetails').hide();
    $('#Appraiser1CommentsFFSelf').removeAttr('disabled');
    $('#Appraiser1CommentsFFEnvi').removeAttr('disabled');
    $('#Appraiser1CommentsIFSelf').removeAttr('disabled');
    $('#Appraiser1CommentsIFEnvi').removeAttr('disabled');
    $('#Appraiser1CommentsSupport').removeAttr('disabled');
}
if ($('#IsManagerOrEmployee').val() == "Appraiser2") {
    $('#btnAddSkillAquiredAppraisalDetails').hide();
    $('#btnAddProjectAchievementsAppraisalDetails').hide();
    $('.containerbtn').hide();
    $('.managerOne').hide();
    $('.appraiserOneHiderating').hide();
    $('.appraiserOneHideComment').hide();
    $('#btnAddCorporateDetailsAppraisal').hide();
    $('#btnAddQualificationAppraisalDetails').hide();
    $('#Appraiser2CommentsFFSelf').removeAttr('disabled');
    $('#Appraiser2CommentsFFEnvi').removeAttr('disabled');
    $('#Appraiser2CommentsIFSelf').removeAttr('disabled');
    $('#Appraiser2CommentsIFEnvi').removeAttr('disabled');
    $('#Appraiser2CommentsSupport').removeAttr('disabled');

    $('#Appraiser2CommentsFFSelf').attr('disabled', false);
    $('#Appraiser2CommentsFFEnvi').attr('disabled', false);
    $('#Appraiser2CommentsIFSelf').attr('disabled', false);
    $('#Appraiser2CommentsIFEnvi').attr('disabled', false);
    $('#Appraiser2CommentsSupport').attr('disabled', false);

    if ($('#IsViewDetails').val() == "Yes") {
        $('#Appraiser2CommentsFFSelf').attr('disabled', 'disabled');
        $('#Appraiser2CommentsFFEnvi').attr('disabled', 'disabled');
        $('#Appraiser2CommentsIFSelf').attr('disabled', 'disabled');
        $('#Appraiser2CommentsIFEnvi').attr('disabled', 'disabled');
        $('#Appraiser2CommentsSupport').attr('disabled', 'disabled');
    }
}

if ($('#IsManagerOrEmployee').val() == "Reviewer1") {
    $('#btnAddSkillAquiredAppraisalDetails').hide();
    $('#btnAddProjectAchievementsAppraisalDetails').hide();
    $('.containerbtn').hide();
    $('.reviewerTwoHiderating').hide();
    $('.reviewerTwoHideComment').hide();
    $('#btnAddCorporateDetailsAppraisal').hide();
    $('#btnAddQualificationAppraisalDetails').hide();
    $('#thReview1Comments').show();
    $('#tdReviewer1CommentsFFSelf').show();
    $('#tdReviewer1CommentsFFEnvi').show();
    $('#tdReviewer1CommentsIFSelf').show();
    $('#tdReviewer1CommentsIFEnvi').show();
    $('#tdReviewer1CommentsSupport').show();
    //        $('#thReview2Comments').show();
    //        $('#tdReviewer2CommentsFFSelf').show();
    //        $('#tdReviewer2CommentsFFEnvi').show();
    //        $('#tdReviewer2CommentsIFSelf').show();
    //        $('#tdReviewer2CommentsIFEnvi').show();
    //        $('#tdReviewer2CommentsSupport').show();
    //        $('#th_reviewerComment2').hide();
    $('#Reviewer2CommentsFFSelf').attr('disabled', true);
    $('#Reviewer2CommentsFFEnvi').attr('disabled', true);
    $('#Reviewer2CommentsIFSelf').attr('disabled', true);
    $('#Reviewer2CommentsIFEnvi').attr('disabled', true);
    $('#Reviewer2CommentsSupport').attr('disabled', true);
    $('#Reviewer1CommentsFFSelf').attr('disabled', false);
    $('#Reviewer1CommentsFFEnvi').attr('disabled', false);
    $('#Reviewer1CommentsIFSelf').attr('disabled', false);
    $('#Reviewer1CommentsIFEnvi').attr('disabled', false);
    $('#Reviewer1CommentsSupport').attr('disabled', false);
    if ($('#IsViewDetails').val() == "Yes") {
        $('#Reviewer1CommentsFFSelf').attr('disabled', 'disabled');
        $('#Reviewer1CommentsFFEnvi').attr('disabled', 'disabled');
        $('#Reviewer1CommentsIFSelf').attr('disabled', 'disabled');
        $('#Reviewer1CommentsIFEnvi').attr('disabled', 'disabled');
        $('#Reviewer1CommentsSupport').attr('disabled', 'disabled');
        $("#NextAppraisal").show();
    }
    $('#btnApprove').hide();
}
if ($('#IsManagerOrEmployee').val() == "Reviewer2") {
    $('#btnAddSkillAquiredAppraisalDetails').hide();
    $('#btnAddProjectAchievementsAppraisalDetails').hide();
    $('.containerbtn').hide();
    $('.reviewerOneHiderating').hide();
    $('.reviewerOneHideComment').hide();
    $('#btnAddCorporateDetailsAppraisal').hide();
    $('#btnAddQualificationAppraisalDetails').hide();
    //        $('#thReview1Comments').show();
    //        $('#tdReviewer1CommentsFFSelf').show();
    //        $('#tdReviewer1CommentsFFEnvi').show();
    //        $('#tdReviewer1CommentsIFSelf').show();
    //        $('#tdReviewer1CommentsIFEnvi').show();
    //        $('#tdReviewer1CommentsSupport').show();
    $('#thReview2Comments').show();
    $('#tdReviewer2CommentsFFSelf').show();
    $('#tdReviewer2CommentsFFEnvi').show();
    $('#tdReviewer2CommentsIFSelf').show();
    $('#tdReviewer2CommentsIFEnvi').show();
    $('#tdReviewer2CommentsSupport').show();
    $('#Reviewer2CommentsFFSelf').attr('disabled', false);
    $('#Reviewer2CommentsFFEnvi').attr('disabled', false);
    $('#Reviewer2CommentsIFSelf').attr('disabled', false);
    $('#Reviewer2CommentsIFEnvi').attr('disabled', false);
    $('#Reviewer2CommentsSupport').attr('disabled', false);
    $('#reject').attr('disabled', false);
    if ($('#IsViewDetails').val() == "Yes") {
        $('#Reviewer2CommentsFFSelf').attr('disabled', 'disabled');
        $('#Reviewer2CommentsFFEnvi').attr('disabled', 'disabled');
        $('#Reviewer2CommentsIFSelf').attr('disabled', 'disabled');
        $('#Reviewer2CommentsIFEnvi').attr('disabled', 'disabled');
        $('#Reviewer2CommentsSupport').attr('disabled', 'disabled');
        $("#NextAppraisal").show();
    }
    $('#btnApprove').hide();
}

if ($('#IsManagerOrEmployee').val() == "GroupHead") {
    /* below id's r for perfHinder*/
    $('#btnAddSkillAquiredAppraisalDetails').hide();
    $('#btnAddProjectAchievementsAppraisalDetails').hide();
    $('.containerbtn').hide();
    $('#btnAddCorporateDetailsAppraisal').hide();
    $('#btnAddQualificationAppraisalDetails').hide();
    $('#thReview1Comments').show();
    $('#tdReviewer1CommentsFFSelf').show();
    $('#tdReviewer1CommentsFFEnvi').show();
    $('#tdReviewer1CommentsIFSelf').show();
    $('#tdReviewer1CommentsIFEnvi').show();
    $('#tdReviewer1CommentsSupport').show();
    $('#thReview2Comments').show();
    $('#tdReviewer2CommentsFFSelf').show();
    $('#tdReviewer2CommentsFFEnvi').show();
    $('#tdReviewer2CommentsIFSelf').show();
    $('#tdReviewer2CommentsIFEnvi').show();
    $('#tdReviewer2CommentsSupport').show();
    $('#thGroupHeadComments').show();
    $('#tdGroupHeadCommentsFFSelf').show();
    $('#tdGroupHeadCommentsFFEnvi').show();
    $('#tdGroupHeadCommentsIFSelf').show();
    $('#tdGroupHeadCommentsIFEnvi').show();
    $('#tdGroupHeadCommentsSupport').show();
    $('#Reviewer2CommentsFFSelf').attr('disabled', true);
    $('#Reviewer2CommentsFFEnvi').attr('disabled', true);
    $('#Reviewer2CommentsIFSelf').attr('disabled', true);
    $('#Reviewer2CommentsIFEnvi').attr('disabled', true);
    $('#Reviewer2CommentsSupport').attr('disabled', true);
    $('#GroupHeadCommentsFFSelf').attr('disabled', false);
    $('#GroupHeadCommentsFFEnvi').attr('disabled', false);
    $('#GroupHeadCommentsIFSelf').attr('disabled', false);
    $('#GroupHeadCommentsIFEnvi').attr('disabled', false);
    $('#GroupHeadCommentsSupport').attr('disabled', false);
    if ($('#IsViewDetails').val() == "Yes") {
        $('#GroupHeadCommentsFFSelf').attr('disabled', 'disabled');
        $('#GroupHeadCommentsFFEnvi').attr('disabled', 'disabled');
        $('#GroupHeadCommentsIFSelf').attr('disabled', 'disabled');
        $('#GroupHeadCommentsIFEnvi').attr('disabled', 'disabled');
        $('#GroupHeadCommentsSupport').attr('disabled', 'disabled');
        $("#NextAppraisal").show();
    }
    $('#btnApprove').hide();
}

if ($('#IsViewDetails').val() == "Yes" || $('#StageID').val() > 3) {
    $('#btnApprove').hide();
    $('#btnSaveAppraisal').hide();
    // $('.button').hide();
    $('#btnRejectAppraisal').hide();
    $('#btnRejectAppraisalReviewer').hide();
    $('.approveReject').hide();
    $('.ui-datepicker-trigger').hide();
    if ($('#IsViewDetails').val() == "Yes" && $('#LinkClicked').val() == "GroupHead" && $('#IsManagerOrEmployee').val() == "GroupHead" && ($('#StageID').val() == 7)) {
        // do nothing
    }
    else {
        $('#frmAppraisalParameter').find('input,tr,textarea').attr('disabled', true);
    }

    $('#frmAddPerfHinderAppraisalDetails').find('input,tr,textarea').attr('disabled', true);
    $('#frmGoalAquireAppraisal').find('input').attr('disabled', true);
    $('#addCorporateContributionDetailsAppraisal').find('input,textarea').attr('disabled', true);
    $('#addAddQualificationAppraisalDetails').find('input,textarea').attr('disabled', true);
    $('#frmProjectAchievementAppraisalDetails').find('input,textarea').attr('disabled', true);
    $('#addSkillsAquiredAppraisalDetails').find('input,textarea').attr('disabled', true);
}

function performanceHinderValidationAppraiser() {
    $('#empCommentsFFSelf').rules("add", {
        required: function () {
            return ($('#empCommentsFFSelf').val() == ' ' || $('#empCommentsFFSelf').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Appraiser1CommentsFFSelf').rules("add", {
        required: function () {
            return ($('#Appraiser1CommentsFFSelf').val() == ' ' || $('#Appraiser1CommentsFFSelf').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Appraiser2CommentsFFSelf').rules("add", {
        required: function () {
            return ($('#Appraiser2CommentsFFSelf').val() == ' ' || $('#Appraiser2CommentsFFSelf').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Reviewer1CommentsFFSelf').rules("add", {
        required: function () {
            return ($('#Reviewer1CommentsFFSelf').val() == ' ' || $('#Reviewer1CommentsFFSelf').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Reviewer2CommentsFFSelf').rules("add", {
        required: function () {
            return ($('#Reviewer2CommentsFFSelf').val() == ' ' || $('#Reviewer2CommentsFFSelf').val() == '');
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
    $('#Appraiser1CommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#Appraiser1CommentsFFEnvi').val() == ' ' || $('#Appraiser1CommentsFFEnvi').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Appraiser2CommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#Appraiser2CommentsFFEnvi').val() == ' ' || $('#Appraiser2CommentsFFEnvi').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Reviewer1CommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#Reviewer1CommentsFFEnvi').val() == ' ' || $('#Reviewer1CommentsFFEnvi').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Reviewer2CommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#Reviewer2CommentsFFEnvi').val() == ' ' || $('#Reviewer2CommentsFFEnvi').val() == '');
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
    $('#Appraiser1CommentsIFSelf').rules("add", {
        required: function () {
            return ($('#Appraiser1CommentsIFSelf').val() == ' ' || $('#Appraiser1CommentsIFSelf').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Appraiser2CommentsIFSelf').rules("add", {
        required: function () {
            return ($('#Appraiser2CommentsIFSelf').val() == ' ' || $('#Appraiser2CommentsIFSelf').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Reviewer2CommentsIFSelf').rules("add", {
        required: function () {
            return ($('#Reviewer2CommentsIFSelf').val() == ' ' || $('#Reviewer2CommentsIFSelf').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Reviewer1CommentsIFSelf').rules("add", {
        required: function () {
            return ($('#Reviewer1CommentsIFSelf').val() == ' ' || $('#Reviewer1CommentsIFSelf').val() == '');
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
    $('#Appraiser1CommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#Appraiser1CommentsIFEnvi').val() == ' ' || $('#Appraiser1CommentsIFEnvi').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Appraiser2CommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#Appraiser2CommentsIFEnvi').val() == ' ' || $('#Appraiser2CommentsIFEnvi').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Reviewer1CommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#Reviewer1CommentsIFEnvi').val() == ' ' || $('#Reviewer1CommentsIFEnvi').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Reviewer2CommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#Reviewer2CommentsIFEnvi').val() == ' ' || $('#Reviewer2CommentsIFEnvi').val() == '');
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
    $('#Appraiser1CommentsSupport').rules("add", {
        required: function () {
            return ($('#Appraiser1CommentsSupport').val() == ' ' || $('#Appraiser1CommentsSupport').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Appraiser2CommentsSupport').rules("add", {
        required: function () {
            return ($('#Appraiser2CommentsSupport').val() == ' ' || $('#Appraiser2CommentsSupport').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });

    $('#Reviewer1CommentsSupport').rules("add", {
        required: function () {
            return ($('#Reviewer1CommentsSupport').val() == ' ' || $('#Reviewer1CommentsSupport').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
    $('#Reviewer2CommentsSupport').rules("add", {
        required: function () {
            return ($('#Reviewer2CommentsSupport').val() == ' ' || $('#Reviewer2CommentsSupport').val() == '');
        },
        messages:
            {
                required: "Comment field is required."
            }
    });
}

function printDivAppraisal() {
    FormatDataForPrintQstn1();
    FormatDataForPrintQstn2();
    FormatDataForPrint();
    FormatDataForPrintQstn4();
    FormatDataForPrintQstn5();
    FormatDataForPrintQstn6();
    FormatDataForPrintQstn7();
    var docprint = window.open("about:blank", "_blank");
    var oTable = document.getElementById("printThisAreaAppraisal");
    docprint.document.open();
    docprint.document.write('<html><head><link rel="stylesheet" type="text/css" href="../../Content/Default.css" /><title>Appraisal Process</title>');
    docprint.document.write('</head><body>');
    docprint.document.write('Appraisee Name :' + employeeName + '</br>');
    docprint.document.write(oTable.parentNode.innerHTML);
    docprint.document.write('</body></html>');
    docprint.document.close();
    docprint.print();
    docprint.close();
}
function printDivAppraisalThree() {
    FormatDataForPrintSection3_1();
    FormatDataForPrintSection3_2_1();
    FormatDataForPrintSection3_2_2();
    FormatDataForPrintSection3_2_3();
    FormatDataForPrintSection_3_3();
    var docprint = window.open("about:blank", "_blank");
    var oTable2 = document.getElementById("printThisDivSectionThree");
    docprint.document.open();
    docprint.document.write('<html><head><link rel="stylesheet" type="text/css" href="../../Content/Default.css" /><title>Appraisal Process</title>');
    docprint.document.write('</head><body>');
    docprint.document.write('Appraisee Name :' + employeeNameAppThree + '</br>');
    docprint.document.write(oTable2.parentNode.innerHTML);
    docprint.document.write('</body></html>');
    docprint.document.close();
    docprint.print();
    docprint.close();
}
function printDivAppraisalFour() {
    var docprint = window.open("about:blank", "_blank");
    var oTable1 = document.getElementById("printThisDivSectionFour");
    docprint.document.open();
    docprint.document.write('<html><head><link rel="stylesheet" type="text/css" href="../../Content/Default.css" /><title>Appraisal Process</title>');
    docprint.document.write('</head><body>');
    docprint.document.write('Appraisee Name :' + employeeNameAppFour + '</br>');
    docprint.document.write(oTable1.parentNode.innerHTML);
    docprint.document.write('</body></html>');
    docprint.document.close();
    docprint.print();
    docprint.close();
}

function saveData(empID, isMngrOrEmpElement, appraisalID, LoggedInEmployeeId, StageID, IsIDFFrozen, isUnfreezedByAdmin) {
    var apprReject = '';
    if ($('#StageID').val() == '6' || $('#StageID').val() == '3') {
        apprReject = 'Approved';
    }

    var isSave = false;
    var statusReturn = SaveDetails(isSave, apprReject, empID, isMngrOrEmpElement, appraisalID, LoggedInEmployeeId, StageID, IsIDFFrozen, isUnfreezedByAdmin);

    return statusReturn;
};

function SaveDetails(isSave, apprReject, empelement, isMngrOrEmpElement, appraisalID, LoggedInEmployeeId, StageID, IsIDFFrozen, isUnfreezedByAdmin) {
    var postUrl = 'SavePerformanceHinderAppraisalInfo/Appraisal';
    var postSecondUrl = 'SaveValueDriverInfoAppraisal/Appraisal';
    var postThirdUrl = 'SaveGoalAspireAppraisal/Appraisal';
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $('textarea').removeClass("input-validation-error");

    if ($('#StageID').val() == '6' || $('#StageID').val() == '3') {
        apprReject = 'Approved';
    }
    if (isSave == false) {  //submit button clicked
        /*VALUE DRIVER VALIDATION START*/
        var vaidationmessaggeRating = true;
        var vaidationmessaggeComment = true;
        var gridCount = true;
        var validationArray = [];
        if (isMngrOrEmpElement != "GroupHead") {
            $('#frmAppraisalParameter').find('tr').each(function () {
                if ($(this).find('.ratingInput').length) {
                    var ratingelement = $(this).find('.ratingInput').not(':disabled').not(':hidden').not('.paramDescription');
                    var commentelement = $(this).find('.commentTxtBox').not(':disabled').not(':hidden').not('.paramDescription');
                    var ratingid = ratingelement.attr("id");
                    var commentid = commentelement.attr("id");
                    //var ratingVal = parseInt(ratingelement.val());
                    var ratingVal = parseInt($(this).find('.ratingInput').next().find('.sbSelector').text());
                    if (ratingVal < '@ViewBag.minRating' || ratingVal > '@ViewBag.maxRating') {
                        var spanid = "Span_" + ratingid;
                        $("#" + spanid).css("display", "block");
                        vaidationmessaggeRating = false;
                        validationArray.push(false);
                    } else {
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
                    } else {
                        var labelid = "Label_" + commentid;
                        $("#" + labelid).css("display", "none");
                        validationArray.push(true);
                        vaidationmessaggeComment = true;
                    }
                } else if ($(this).find('.footer-rating-class').not(':disabled').not(':hidden').length) {
                    ratingelement = $(this).find('.footer-rating-class').not(':disabled').not(':hidden');
                    commentelement = $(this).find('.footer-comment-TxtBox').not(':disabled').not(':hidden');
                    //ratingVal = parseInt(ratingelement.val());
                    ratingVal = parseInt($(this).find('.footer-rating-class').next().find('.sbSelector').text());
                    ratingid = ratingelement.attr("id");
                    if (ratingVal < '@ViewBag.minRating' || ratingVal > '@ViewBag.maxRating') {
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
        }
        /*VALUE DRIVER VALIDATION END*/

        //            $('#btnPerfHinderSubmit').submit(); // Performance Hinder Validation.
        $('#btnPerfHinderSubmit').submit(function () {
            var apprReject = '';
            if ($('#StageID').val() == '6' || $('#StageID').val() == '3') {
                apprReject = 'Approved';
            }
        });

        /*Corporate Contribution Validation*/
        var localGridData = jQuery("#corporateTableAppraisal").getRowData();   // removed validation for appraiser,reviwer comments
        var corparteValidationArr = true;
        for (var i = 0; i < localGridData.length; i++) {
            if ($('#IsManagerOrEmployee').val() == "Appraiser1") {
                var mangrComments = localGridData[i].Appraiser1Comments;
                if (mangrComments == "") {
                    corparteValidationArr = false;
                }
            } else if ($('#IsManagerOrEmployee').val() == "Appraiser2") {
                var mangrCommentsSecond = localGridData[i].Appraiser2Comments;
                if (mangrCommentsSecond == "") {
                    corparteValidationArr = false;
                }
            } else if ($('#IsManagerOrEmployee').val() == "Reviewer1") {
                var reviewComments = localGridData[i].Reviewer1Comments;
                if (reviewComments == "") {
                    corparteValidationArr = false;
                }
            } else if ($('#IsManagerOrEmployee').val() == "Reviewer2") {
                var reviewComments = localGridData[i].Reviewer2Comments;
                if (reviewComments == "") {
                    corparteValidationArr = false;
                }
            }
            //            else if ($('#IsManagerOrEmployee').val() == "GroupHead") {
            //                var hrRevComments = localGridData[i].GrpHeadComments;
            //                if (hrRevComments == "") {
            //                    corparteValidationArr = false;
            //                }
            //            }
        } /*End*/

        if ($('#IsManagerOrEmployee').val() == "Employee") {
            $("#btnSubmit").submit();
        }
        $.each($('#frmAppraisalParameter input, #frmAppraisalParameter select, #frmAppraisalParameter textarea').serializeArray(), function (i, obj) {
            $('<input type="hidden">').prop(obj).appendTo($('#frmAddPerfHinderAppraisalDetails'));
        });

        if ($('#IsManagerOrEmployee').val() == "Employee") {
            var gridCount = true;

            if ($("#projectAchievementsAppraisalTable").getGridParam("reccount") == 0) {
                gridCount = false;
                $('#ProjAchievementRecordAppraisal').dialog({
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
                $('#ProjAchievementRecordAppraisal').dialog('open');
                $.preventDefault();
            }

            if ($("#corporateTableAppraisal").getGridParam("reccount") == 0) {
                gridCount = false;
                $("#CorporateRecordAppraisal").dialog({
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
                $.preventDefault();
            }
            if ($("#SkillAquiredAppraisalTable").getGridParam("reccount") == 0) {
                gridCount = false;
                $('#SkillRecordAppraisal').dialog({
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
                $('#SkillRecordAppraisal').dialog('open');
                $.preventDefault();
            }

            if ($("#AddQualificationAppraisalTable").getGridParam("reccount") == 0) {
                gridCount = false;
                $('#QualificationRecordAppraisal').dialog({
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
                $('#QualificationRecordAppraisal').dialog('open');
                $.preventDefault();
            }

            if ($('#check').val() != 0 || $('#checkHinders').val() != 0 || gridCount == false) {
                return false;
            }
        }
        if (corparteValidationArr == false) {
            $('#CorporateRecordAppraisal').text('Please enter comments for corporate contribution');
            $("#CorporateRecordAppraisal").dialog({
                autoopen: true,
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
        if (!$('#frmAddPerfHinderAppraisalDetails').valid()) {
            var message = "Please fill all the mandatory fields from Question number 3."
            ShowValidationDialog(message);
            return false;
        }

        if (!$('#frmAppraisalParameter').valid()) {
            var message = "Please fill all the mandatory fields from Question number 7."
            ShowValidationDialog(message);
            return false;
        }

        if (!$('#frmGoalAquireAppraisal').valid()) {
            var message = "Please fill all the mandatory fields from Question number 6."
            ShowValidationDialog(message);
            return false;
        }
    } //save end
    DisplayLoadingDialog(); // Checked global.js

    //        if ($('#frmAddPerfHinderAppraisalDetails').valid())
    {
        $.ajax({
            url: postUrl,
            type: 'POST',
            async: false,
            data: $('#frmAddPerfHinderAppraisalDetails').serialize(),
            success: function (results) {
                if (results.status == true) {
                    var myArray = [];
                    //                    var empelement = '@ViewBag.EmployeeID';
                    //                    var isMngrOrEmpElement = $('#IsManagerOrEmployee').val();
                    //                    var appraisalID = '@ViewBag.appraisalId';

                    $('#frmAppraisalParameter').find('tr').each(function (index) {
                        var thisparent = $(this);
                        if ($(this).find('.ratingInput').length) {
                            var ratingelement = thisparent.find('.ratingInput').not(':disabled').not(':hidden').not('.paramDescription');
                            var commentelement = thisparent.find('.commentTxtBox').not(':disabled').not(':hidden').not('.paramDescription');
                            var descriptionelement = thisparent.find('.paramDescription').val();
                            var competencyelement = thisparent.find('.competency').val();
                            if ($('#IsManagerOrEmployee').val() == "Employee") {
                                var ConfirmationParameter = {
                                    SelfRating: thisparent.find('.ratingInput').next().find('.sbSelector').text(),
                                    EmpComments: commentelement.val(),
                                    parameterID: competencyelement,
                                    employeeID: empelement,
                                    IsManagerOrEmployee: isMngrOrEmpElement,
                                    appraisalId: appraisalID
                                };
                                myArray.push(ConfirmationParameter);
                            }
                            if ($('#IsManagerOrEmployee').val() == "Appraiser1") {
                                var ConfirmationParameter = {
                                    AppraiserRating1: thisparent.find('.ratingInput').next().find('.sbSelector').text(),
                                    AppraiserComments1: commentelement.val(),
                                    parameterID: competencyelement,
                                    employeeID: empelement,
                                    IsManagerOrEmployee: isMngrOrEmpElement,
                                    appraisalId: appraisalID
                                };
                                myArray.push(ConfirmationParameter);
                            }
                            if ($('#IsManagerOrEmployee').val() == "Appraiser2") {
                                var ConfirmationParameter = {
                                    AppraiserRating2: thisparent.find('.ratingInput').next().find('.sbSelector').text(),
                                    AppraiserComments2: commentelement.val(),
                                    parameterID: competencyelement,
                                    employeeID: empelement,
                                    IsManagerOrEmployee: isMngrOrEmpElement,
                                    appraisalId: appraisalID
                                };
                                myArray.push(ConfirmationParameter);
                            }
                            if ($('#IsManagerOrEmployee').val() == "Reviewer1") {
                                var ConfirmationParameter = {
                                    ReviewerRating1: thisparent.find('.ratingInput').next().find('.sbSelector').text(),
                                    ReviewerComments1: commentelement.val(),
                                    parameterID: competencyelement,
                                    employeeID: empelement,
                                    IsManagerOrEmployee: isMngrOrEmpElement,
                                    appraisalId: appraisalID
                                };
                                myArray.push(ConfirmationParameter);
                            }
                            if ($('#IsManagerOrEmployee').val() == "Reviewer2") {
                                var ConfirmationParameter = {
                                    ReviewerRating2: thisparent.find('.ratingInput').next().find('.sbSelector').text(),
                                    ReviewerComments2: commentelement.val(),
                                    parameterID: competencyelement,
                                    employeeID: empelement,
                                    IsManagerOrEmployee: isMngrOrEmpElement,
                                    appraisalId: appraisalID
                                };
                                myArray.push(ConfirmationParameter);
                            }
                            if ($('#IsManagerOrEmployee').val() == "GroupHead") {
                                var ConfirmationParameter = {
                                    GrpHeadRating: thisparent.find('.ratingInput').next().find('.sbSelector').text(),
                                    GrpHeadComments: commentelement.val(),
                                    parameterID: competencyelement,
                                    employeeID: empelement,
                                    IsManagerOrEmployee: isMngrOrEmpElement,
                                    appraisalId: appraisalID
                                };
                                myArray.push(ConfirmationParameter);
                            }
                        }
                        if ($(this).find('.footer-rating-class').length) {
                            if ($('#IsManagerOrEmployee').val() == "Reviewer1") {
                                var overallratingelement = thisparent.find('.footer-rating-class').not(':disabled').not(':hidden');
                                var overallcommentelement = thisparent.find('.footer-comment-TxtBox').not(':disabled').not(':hidden');
                                var ConfirmationParameter = {
                                    OverallReviewRating: thisparent.find('.footer-rating-class').next().find('.sbSelector').text(),
                                    OverallReviewRatingComments: overallcommentelement.val(),
                                    appraisalId: appraisalID
                                };
                                myArray.push(ConfirmationParameter);
                            }
                            if ($('#IsManagerOrEmployee').val() == "Reviewer2") {
                                var overallratingelement = thisparent.find('.footer-rating-class').not(':disabled').not(':hidden');
                                var overallcommentelement = thisparent.find('.footer-comment-TxtBox').not(':disabled').not(':hidden');
                                var ConfirmationParameter = {
                                    OverallReview2Rating: thisparent.find('.footer-rating-class').next().find('.sbSelector').text(),
                                    OverallReview2RatingComments: overallcommentelement.val(),
                                    appraisalId: appraisalID
                                };
                                myArray.push(ConfirmationParameter);
                            }
                            if ($('#IsManagerOrEmployee').val() == "GroupHead") {
                                var overallratingelementGrp = thisparent.find('#OverallGrpHeadRating').not(':disabled').not(':hidden');
                                var overallcommentelement = thisparent.find('.footer-comment-TxtBox').not(':disabled').not(':hidden');
                                var ConfirmationParameter = {
                                    OverallGrpHeadRating: thisparent.find('.footer-rating-class').next().find('.sbSelector').text(),
                                    OverallGrpHeadComments: overallcommentelement.val(),
                                    appraisalId: appraisalID,
                                    LoggedInEmployeeId: LoggedInEmployeeId,
                                    StageID: StageID,
                                    IsIDFFrozen: IsIDFFrozen,
                                    isUnfreezedByAdmin: isUnfreezedByAdmin
                                };
                                myArray.push(ConfirmationParameter);
                            }
                        }
                    });
                    var datasecond = JSON.stringify(myArray);

                    //                        if ($('#frmAppraisalParameter').valid())
                    {
                        $.ajax({
                            type: 'POST',
                            url: postSecondUrl,
                            data: JSON.stringify(myArray),
                            contentType: "application/json; charset=utf-8",
                            success: function (result) {
                                if (result.status == true) {
                                    //                                    $("#loading").dialog("close");
                                    //                                    $("#loading").dialog("destroy");
                                    $('#frmGoalAquireAppraisal').find('textarea').attr('disabled', false);
                                    var GoalAquireAppraisal = {
                                        EmployeIDGoal: employeeId,
                                        AppraisalIDGoal: appraisalID,
                                        ShortTerm: $('#ShortTerm').val(),
                                        LongTerm: $('#LongTerm').val(),
                                        SkillDevPrgm: $('#SkillDevPrgm').val(),
                                        IsManagerOrEmployee: $('#IsManagerOrEmployee').val()
                                    }

                                    $.ajax({
                                        type: 'POST',
                                        url: postThirdUrl,
                                        data: JSON.stringify(GoalAquireAppraisal),
                                        contentType: "application/json; charset=utf-8",
                                        success: function (output) {
                                            if (output.status == true) {
                                                $("#loading").dialog("close");
                                                $("#loading").dialog("destroy");
                                                $("#SuccessAppraisalProcess").dialog({
                                                    resizable: false,
                                                    height: 140,
                                                    width: 300,
                                                    modal: true,
                                                    title: "Success Message",
                                                    dialogClass: "noclose",
                                                    buttons: {
                                                        Ok: function () {
                                                            if ($('#IsManagerOrEmployee').val() == "GroupHead" && (StageID == 7 || StageID == 8 || IsIDFFrozen == true) && isUnfreezedByAdmin == "True") {
                                                                $("#frmAddPerfHinderAppraisalDetails").find('input,tr,td,textarea,select').attr("disabled", "disabled");
                                                                $("#frmGoalAquireAppraisal").find('input,tr,td,textarea,select').attr("disabled", "disabled");
                                                            }
                                                            else if ($('#IsManagerOrEmployee').val() == 'Reviewer1' || $('#IsManagerOrEmployee').val() == 'Reviewer2' || $('#IsManagerOrEmployee').val() == 'GroupHead') {
                                                                $('#NextAppraisal').show();
                                                            }
                                                            $(this).dialog("close");
                                                        }
                                                    }
                                                });
                                            }
                                            else if (output.status == "Error") {
                                                $("#loading").dialog("close");
                                                $("#loading").dialog("destroy");
                                                DisplayErrorDialog("Appraisal Process");
                                            }
                                            else {
                                                $("#loading").dialog("close");
                                                $("#loading").dialog("destroy");
                                                $("#AddRecordErrorMessege").dialog({
                                                    resizable: false,
                                                    height: 140,
                                                    width: 300,
                                                    modal: true,
                                                    title: "Error",
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
                                        $('#frmGoalAquireAppraisal').find('input,select,textarea').attr('disabled', false);
                                    }
                                    else {
                                        $('#frmGoalAquireAppraisal').find('input,select,textarea').attr('disabled', true);
                                    }
                                }
                                else if (result.status == "Error") {
                                    $("#loading").dialog("close");
                                    $("#loading").dialog("destroy");
                                    DisplayErrorDialog("Appraisal Process");
                                }
                                else {
                                    $("#loading").dialog("close");
                                    $("#loading").dialog("destroy");
                                    $("#AddPerformanceErrorMessege").dialog({
                                        resizable: false,
                                        height: 140,
                                        width: 300,
                                        modal: true,
                                        title: "Error",
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
                else if (results.status == "Error") {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    DisplayErrorDialog("Appraisal Process");
                }
                else {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    $("#AddPerformanceErrorMessege").dialog({
                        resizable: false,
                        height: 140,
                        width: 300,
                        modal: true,
                        title: "Error",
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
    //    $("#loading").dialog("close");
    //    $("#loading").dialog("destroy");
    if ($("#errorDialog").dialog("isOpen") == true) {
        return false;
    }
    else {
        return true;
    }
};

function ShowValidationDialog(validationMessage) {
    $("#ShowValidationDialog span").html("");
    $("#ShowValidationDialog span").append(validationMessage);
    $("#ShowValidationDialog").dialog({
        resizable: false,
        height: 140,
        width: 300,
        modal: true,
        title: "Required field Validation",
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        }
    });
}

function ISFormSubmitedorNotCorporate() {
    var appraisalID = $('#appraisalId').val();
    var isMngrOrEmpElement = $('#IsManagerOrEmployee').val();

    var postUrl = "IsFormSumitedorNot/Appraisal";
    $.ajax({
        url: postUrl,
        type: 'POST',
        async: false,
        data: { isMngrOrEmpElement: isMngrOrEmpElement, appraisalID: appraisalID },
        success: function (data) {
            if (data.status == true) {
                $("#ErrorSumitedForAdmin").dialog({
                    resizable: false,
                    height: 140,
                    width: 300,
                    modal: true,
                    buttons: {
                        Ok: function () {
                            $(this).dialog("destroy");
                            window.location.href = '@Url.Action("AppraisalProcessStatus", "Appraisal")';
                        }
                    }
                });
                $.preventDefault();
            }
        }
    });
}