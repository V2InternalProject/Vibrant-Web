//no children changes
function NoChildrenchange() {
    if ($("#NoOfchildren").val() == 0) {
        $(".ChildBDDIV").hide();
        $("#children1BirthDateCollection .ui-datepicker-trigger").hide();
        $("#children2BirthDateCollection .ui-datepicker-trigger").hide();
        $("#children1Name").attr("disabled", "disabled");
        $("#children1BirthDate").attr("disabled", "disabled");
        $("#children2Name").attr("disabled", "disabled");
        $("#children2BirthDate").attr("disabled", "disabled");

        $("#children1Name").val('');
        $("#children1BirthDate").val('');
        $("#children2Name").val('');
        $("#children2BirthDate").val('');
        $("#children3Name").val('');
        $("#children3BirthDate").val('');
        $("#children4Name").val('');
        $("#children4BirthDate").val('');
        $("#children5Name").val('');
        $("#children5BirthDate").val('');
    }
    else if ($("#NoOfchildren").val() == 1) {
        $(".ChildBDDIV").show();
        $(".divFirstchild").show();
        $(".divSeconChild").hide();
        $(".divThirdChild").hide();
        $(".divFourthChild").hide();
        $(".divFifthChild").hide();
        $("#children1Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children1BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children1BirthDateCollection .ui-datepicker-trigger").show();
        $("#children1Name").val($("#children1name").val());
        $("#children1BirthDate").val($("#children1birthdate").val());
        $("#children2Name").val('');
        $("#children2BirthDate").val('');
        $("#children3Name").val('');
        $("#children3BirthDate").val('');
        $("#children4Name").val('');
        $("#children4BirthDate").val('');
        $("#children5Name").val('');
        $("#children5BirthDate").val('');
    }
    else if ($("#NoOfchildren").val() == 2) {
        $(".ChildBDDIV").show();
        $(".divFirstchild").show();
        $(".divSeconChild").show();
        $(".divThirdChild").hide();
        $(".divFourthChild").hide();
        $(".divFifthChild").hide();

        $("#children1Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children1BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children2Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children2BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children1BirthDateCollection .ui-datepicker-trigger").show();
        $("#children2BirthDateCollection .ui-datepicker-trigger").show();

        $("#children1Name").val($("#children1name").val());
        $("#children1BirthDate").val($("#children1birthdate").val());

        $("#children2Name").val($("#children2name").val());
        $("#children2BirthDate").val($("#children2birthdate").val());
        $("#children3Name").val('');
        $("#children3BirthDate").val('');
        $("#children4Name").val('');
        $("#children4BirthDate").val('');
        $("#children5Name").val('');
        $("#children5BirthDate").val('');
    }

    else if ($("#NoOfchildren").val() == 3) {
        $(".ChildBDDIV").show();
        $(".divFirstchild").show();
        $(".divSeconChild").show();

        $(".divThirdChild").show();

        $(".divFourthChild").hide();
        $(".divFifthChild").hide();

        $("#children1Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children1BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children2Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children3Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children2BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children3BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children1BirthDateCollection .ui-datepicker-trigger").show();
        $("#children2BirthDateCollection .ui-datepicker-trigger").show();
        $("#children3BirthDateCollection .ui-datepicker-trigger").show();

        $("#children1Name").val($("#children1name").val());
        $("#children1BirthDate").val($("#children1birthdate").val());;

        $("#children2Name").val($("#children2name").val());
        $("#children2BirthDate").val($("#children2birthdate").val());;

        $("#children3Name").val($("#children3name").val());
        $("#children3BirthDate").val($("#children3birthdate").val());
        $("#children4Name").val('');
        $("#children4BirthDate").val('');
        $("#children5Name").val('');
        $("#children5BirthDate").val('');
    }

    else if ($("#NoOfchildren").val() == 4) {
        $(".ChildBDDIV").show();
        $(".divFirstchild").show();
        $(".divSeconChild").show();
        $(".divThirdChild").show();
        $(".divFourthChild").show();
        $(".divFifthChild").hide();
        $("#children1Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children1BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children2Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children3Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();

        $("#children4Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();

        $("#children2BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children3BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();

        $("#children4BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();

        $("#children1BirthDateCollection .ui-datepicker-trigger").show();
        $("#children2BirthDateCollection .ui-datepicker-trigger").show();
        $("#children3BirthDateCollection .ui-datepicker-trigger").show();

        $("#children4BirthDateCollection .ui-datepicker-trigger").show();

        $("#children1Name").val($("#children1name").val());
        $("#children1BirthDate").val($("#children1birthdate").val());;

        $("#children2Name").val($("#children2name").val());
        $("#children2BirthDate").val($("#children2birthdate").val());;

        $("#children3Name").val($("#children3name").val());
        $("#children3BirthDate").val($("#children3birthdate").val());

        $("#children4Name").val($("#children4name").val());
        $("#children4BirthDate").val($("#children4birthdate").val());

        $("#children5Name").val('');
        $("#children5BirthDate").val('');
    }

    else {
        $(".ChildBDDIV").show();
        $(".divSeconChild").show();
        $("#divFirstChild").show();
        $(".divThirdChild").show();
        $(".divFourthChild").show();
        $(".divFifthChild").show();

        $("#children1Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children1BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children2Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children3Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();

        $("#children4Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children5Name").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();

        $("#children2BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children3BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();

        $("#children4BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();
        $("#children5BirthDate").removeAttr('disabled').show().next(".ClassDisplayLabel").hide();

        $("#children1BirthDateCollection .ui-datepicker-trigger").show();
        $("#children2BirthDateCollection .ui-datepicker-trigger").show();
        $("#children3BirthDateCollection .ui-datepicker-trigger").show();

        $("#children4BirthDateCollection .ui-datepicker-trigger").show();
        $("#children5BirthDateCollection .ui-datepicker-trigger").show();

        $("#children1Name").val($("#children1name").val());
        $("#children1BirthDate").val($("#children1birthdate").val());;

        $("#children2Name").val($("#children2name").val());
        $("#children2BirthDate").val($("#children2birthdate").val());;

        $("#children3Name").val($("#children3name").val());
        $("#children3BirthDate").val($("#children3birthdate").val());

        $("#children4Name").val($("#children4name").val());
        $("#children4BirthDate").val($("#children4birthdate").val());
        $("#children5Name").val($("#children5name").val());
        $("#children5BirthDate").val($("#children5birthdate").val());
    }
}

//read image
function readImage(input) {
    var path = input.value.replace("C:\\fakepath\\", "");
    $("#FileBlogPicField").val(path);
    var file = $('input[type="file"]').val();
    var exts = ['gif', 'png', 'jpg', 'jpeg'];
    // first check if file field has any value
    if (file) {
        // split file name at dot
        var get_ext = file.split('.');
        // reverse name to check extension
        get_ext = get_ext.reverse();
        // check file type is valid as given in 'exts' array
        if ($.inArray(get_ext[0].toLowerCase(), exts) > -1) {
            if (!window.FileReader || $.browser.version == '8.0' || $.browser.version == '9.0') {
                var postUrl = "GetFileFromPath/PersonalDetails";
                $.ajax({
                    url: postUrl,
                    data: { filePath: $('#blogpic').val() },
                    success: function (results) {
                        $('#PersonalImagePreview').attr("src", "data:image/jpg;base64," + results);
                    }
                });
                return false;
            }
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#PersonalImagePreview').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }
        }
        else {
            $("#ImageUploadError").dialog({
                title: 'Error',
                resizable: false,
                height: 'auto',
                width: 'auto',
                modal: true,
                dialogClass: 'noclose',
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                        $('#blogpic').val('');
                    }
                }
            });
        }
    }
}

function userlogin() {
    $("#ContractEmployeeChk").attr("disabled", "disabled");
    $("#EmployeeCode").attr("disabled", "disabled");
    $("#ContractFrom").attr("disabled", "disabled");
    $("#ContractTo").attr("disabled", "disabled");
    //    $("#Prefix").attr("disabled", "disabled");
    //    $("#FirstName").attr("disabled", "disabled");
    //    $("#MiddleName").attr("disabled", "disabled");
    //    $("#LastName").attr("disabled", "disabled");
    $("#UserName").attr("disabled", "disabled");
    $("#AgreementDate").attr("disabled", "disabled");
    $("#Gender").attr("disabled", "disabled");
    $("#birthDate").attr("disabled", "disabled");
    $("#Age").attr("disabled", "disabled");
    $("#DateOfBirthCollection .ui-datepicker-trigger").hide();
    $("span.ApprovalMessage").hide();
}

//change event of Birthdate
function birthChange() {
    var todate = new Date();
    var fromDate = new Date($('#birthDate').val());
    var retToDate = todate.getFullYear() * 12 + todate.getMonth();
    var retFromDate = fromDate.getFullYear() * 12 + fromDate.getMonth();
    var monthDiff = (retToDate - retFromDate) / 12;
    var absMonth = Math.floor(monthDiff);
    var vYrs = absMonth + "yrs"
    $('#Age').val(vYrs);
}

//blur event of lastName
function lastNameBlur() {
    var fname = $('#FirstName').val();
    var lname = $('#LastName').val();
    if ($('#UserName').val() == '') {
        $('#UserName').removeAttr("disabled");
        $('#UserName').attr("enabled");
    }
    var username = $('#UserName').attr("disabled");
    if (username != "disabled") {
        if (fname.length != 0 && lname.length != 0) {
            var firstName = fname.toLowerCase().replace(/[^a-z_.]/gi, '');
            var lastName = lname.toLowerCase().replace(/[^a-z_.]/gi, '');
            var uname = null;
            if (firstName == "" || lastName == "")
                uname = firstName + lastName;
            else
                uname = firstName + "." + lastName;
            $('#UserName').val(uname);
            $('#UserName').focusout();
        }
    }
}

//blur event of firstname
function firstNameBlur() {
    var fname = $('#FirstName').val();
    var lname = $('#LastName').val();
    if ($('#UserName').val() == '') {
        $('#UserName').removeAttr("disabled");
    }

    var username = $('#UserName').attr("disabled");
    if (username != "disabled") {
        if (fname.length != 0 && lname.length != 0) {
            var firstName = fname.toLowerCase().replace(/[^a-z_.]/gi, '');
            var lastName = lname.toLowerCase().replace(/[^a-z_.]/gi, '');
            var uname = null;
            if (firstName == "" || lastName == "")
                uname = firstName + lastName;
            else
                uname = firstName + "." + lastName;
            $('#UserName').val(uname);
            $('#UserName').focusout();
        }
    }
}

function callPersonalDetailsMail(employeeID) {
    $.ajax({
        url: "MailSend/PersonalDetails",
        type: 'POST',
        async: false,
        data: { employeeId: employeeID, Module: "Personal Details" },
        success: function (data) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            //            $("#successDialog").dialog('destroy');
            if (data.validCcId == true && data.validtoId == true) {
                if (data.status == true) {
                    $("#mailSendSuccess").dialog({
                        closeOnEscape: false,
                        resizable: false,
                        height: 'auto',
                        title: "Mail Sent",
                        width: 300,
                        modal: true,
                        dialogClass: 'noclose',
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                                window.location.reload();
                            }
                        }
                    });
                }
            }
            else if (data.status == "Error") {
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    title: 'Mail Error',
                    dialogClass: 'noclose',
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                            window.location.reload();
                        }
                    }
                }); //end dialog
            }
            else if (data.status == "ErrorRecipient") {
                $("#failedRecipient #span_failedRecipient").append(data.failedRecipient);
                $("#failedRecipient").dialog({
                    closeOnEscape: false,
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    modal: true,
                    dialogClass: 'noclose',
                    buttons: {
                        Ok: function () {
                            $(this).dialog('close');
                            window.location.reload();
                        }
                    }
                });
            }
            else {
                if (data.validCcId == false) {
                    $("#InvalidEmail").dialog({
                        closeOnEscape: false,
                        resizable: false,
                        height: 'auto',
                        width: 300,
                        modal: true,
                        dialogClass: 'noclose',
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                                window.location.reload();
                            }
                        }
                    });
                }
                if (data.validtoId == false) {
                    $("#InvalidEmail").dialog({
                        closeOnEscape: false,
                        resizable: false,
                        height: 'auto',
                        width: 300,
                        modal: true,
                        dialogClass: 'noclose',
                        buttons: {
                            Ok: function () {
                                $(this).dialog('close');
                                window.location.reload();
                            }
                        }
                    });
                }
                return false;
            }
        }, // end success
        error: function () {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#successDialog").dialog('destroy');
            $("#mailError").dialog({
                resizable: false,
                height: 'auto',
                width: 'auto',
                modal: true,
                title: 'Mail Error',
                dialogClass: 'noclose',
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                        window.location.reload();
                    }
                }
            });
        }
    });
}