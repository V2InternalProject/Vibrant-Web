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
function ConfigureAppraisalYear(postUrl) {
    if ($("#ConfigNewAppYearFrm").valid()) {
        var selectedYearId = $("#Year").val();
        DisplayLoadingDialog();  // checked
        $.ajax({
            url: postUrl,
            type: 'POST',
            data: $('#ConfigNewAppYearFrm').serialize(),
            success: function (results) {
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
                if (results.status == true) {
                    window.location = "/ConfigurationAppraisal/AppraisalParameters?AppraisalYearID=" + selectedYearId;
                }
                else if (results.status == "Error") {
                    $("#errorDialog").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: 'Configure Appraisal',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    }); //end dialog
                }
                else {
                    $("#errorUpdateAppYear").dialog({
                        resizable: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        title: 'Configure Appraisal',
                        dialogClass: "noclose",
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    }); //end dialog
                }
            },
            error: function (results) {
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Configure Appraisal',
                    dialogClass: "noclose",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                }); //end dialog
                $("#loading").dialog("close");
                $("#loading").dialog("destroy");
            }
        });

        //window.location.href = '@Url.Action("AppraisalParameters", "ConfigurationAppraisal")';
    }
    else
        return false;
}