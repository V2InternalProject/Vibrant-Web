if (isManagerOrEmp == "Employee") {
    $('.spanEmployee').show();
}
$('#ContributionDesc').attr('title', $('#ContributionDesc').val());

if (isManagerOrEmp == "Manager") {
    $('#AreaOfContribution').attr('disabled', true);
    $('#ContributionDesc').attr('disabled', true);
    if ('@ViewBag.clickedViewDetails' == "yes")
        $('#ManagerComments').attr('disabled', true);
    else
        $('#ManagerComments').attr('disabled', false);
    $('#ManagerCommentsSecond').attr('disabled', true);
    $('#HRReviewerComments').hide();
    $('#ReviewerComments').hide();
    $('.labelHRReviewerComments').hide();
    $('.labelReviewerComments').hide();
    $('.btnCorporate').show();
    $('.btnQual').hide();
    $('.btnSkill').hide();
    $('.btnReset').show();
    $('.spanManager').show();
}
if (isManagerOrEmp == "Manager2") {
    $('#AreaOfContribution').attr('disabled', true);
    $('#ContributionDesc').attr('disabled', true);
    if ('@ViewBag.clickedViewDetails' == "yes")
        $('#ManagerCommentsSecond').attr('disabled', true);
    else
        $('#ManagerCommentsSecond').attr('disabled', false);
    $('#ManagerComments').attr('disabled', true);
    $('#HRReviewerComments').hide();
    $('#ReviewerComments').hide();
    $('.labelHRReviewerComments').hide();
    $('.labelReviewerComments').hide();
    $('.btnCorporate').show();
    $('.btnQual').hide();
    $('.btnReset').show();
    $('.spanManager2').show();
}

if (isManagerOrEmp == "Reviewer") {
    $('#AreaOfContribution').attr('disabled', true);
    $('#ContributionDesc').attr('disabled', true);
    if ('@ViewBag.clickedViewDetails' == "yes")
        $('#ReviewerComments').attr('disabled', true);
    else
        $('#ReviewerComments').attr('disabled', false);
    $('#HRReviewerComments').show();
    $('#ManagerComments').show();
    $('#ManagerComments').attr('disabled', true);
    $('#ManagerCommentsSecond').attr('disabled', true);
    $('.labelHRReviewerComments').show();
    $('.btnCorporate').show();
    $('.btnQual').hide();
    $('.btnSkill').hide();
    $('.btnReset').show();
    $('.spanReviewer').show();
}
if (isManagerOrEmp == "HR" && stageId != 6) {
    $('#AreaOfContribution').attr('disabled', true);
    $('#ContributionDesc').attr('disabled', true);
    if ('@ViewBag.clickedViewDetails' == "yes")
        $('#HRReviewerComments').attr('disabled', true);
    else
        $('#HRReviewerComments').attr('disabled', false);
    $('#ReviewerComments').show();
    $('#ManagerComments').show();
    $('#ManagerComments').attr('disabled', true);
    $('#ManagerCommentsSecond').attr('disabled', true);
    $('#ReviewerComments').attr('disabled', true);
    $('.btnCorporate').show();
    $('.btnQual').hide();
    $('.btnSkill').hide();
    $('.btnReset').show();
    $('.spanHR').show();
}
if (stageId == 6) {
    $('#AreaOfContribution').attr('disabled', true);
    $('#ContributionDesc').attr('disabled', true);
    $('.btnCorporate').hide();
    $('.btnReset').hide();
}
$('#AreaOfContribution').rules("add", {
    required: function () {
        return (isManagerOrEmp == "Employee");
    },
    messages:
			{
			    required: "Area Of Contribution is required."
			}
});

$('#ContributionDesc').rules("add", {
    required: function () {
        return (isManagerOrEmp == "Employee");
    },
    messages:
			{
			    required: "Contribution Description is required."
			}
});

$('#ManagerComments').rules("add", {
    required: function () {
        return (isManagerOrEmp == "Manager");
    },
    messages:
			{
			    required: "Manager Comments is required."
			}
});

$('#ReviewerComments').rules("add", {
    required: function () {
        return (isManagerOrEmp == "Reviewer");
    },
    messages:
			{
			    required: "Reviewer Comments is required."
			}
});

$('#HRReviewerComments').rules("add", {
    required: function () {
        return (isManagerOrEmp == "HR");
    },
    messages:
			{
			    required: "HR's Reviewer Comments is required."
			}
});

$('#savedependant').off('click').on('click', function () {
    if ($('#addCorporateContributionDetails').valid()) {
        $.ajax({
            url: 'SaveCorporateInfo',
            type: 'POST',
            data: $('#addCorporateContributionDetails').serialize(),
            success: function (results) {
                if (results.status == true) {
                    $('#addCorporateDialog').dialog("close");
                    jQuery("#corporateTable").trigger("reloadGrid");
                    $("#AddCorporateSuccessMessege").dialog({
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
                else if (results.status == "Error") {
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
                    $("#AddCorporateErrorMessege").dialog({
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
    }
    return false;
});

var emptyDialogdependant = function () {
    $(".field-validation-error").empty();
    $('input').removeClass("input-validation-error");
    $("#AreaOfContribution").val($('#areaOfContribution').val());
    $("#ContributionDesc").val($('#txtContributionDesc').val());
    $("#ManagerComments").val($('#txtManagerComments').val());
    $("#ManagerCommentsSecond").val($('#txtManagerCommentsSecond').val());
    $("#ReviewerComments").val($('#txtReviewerComments').val());
    $("#HRReviewerComments").val($('#txtHRReviewerComments').val());
}