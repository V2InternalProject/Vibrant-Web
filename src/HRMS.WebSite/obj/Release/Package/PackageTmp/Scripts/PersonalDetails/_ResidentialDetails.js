//to mark as a same address
function MarkSameAddress() {
    if ($('#chkAddress').is(':checked')) {
        var currentadd = $("#txtCurrentAddress").val();
        var currentState = $("#txtCurrentState").val();
        var currentCity = $("#txtCurrentCity").val();
        var currentZipCode = $("#txtCurrentZipCode").val();

        $("#txtAddress").val(currentadd);
        $("#txtState").val(currentState);
        $("#txtCity").val(currentCity);
        $("#txtZipCode").val(currentZipCode);
        $("#ddlCountry")[0].selectedIndex = $("#ddlCurrentCountry")[0].selectedIndex;

        $("#txtAddress").attr('readonly', 'readonly');
        $("#txtState").attr('readonly', 'readonly');
        $("#txtCity").attr('readonly', 'readonly');
        $("#txtZipCode").attr('readonly', 'readonly');
        $("#Permenant").attr('disabled', true);
    }
    else {
        $("#txtAddress").val('');
        $("#txtState").val('');
        $("#txtCity").val('');
        $("#txtZipCode").val('');
        $("#ddlCountry").val("Select");
        $("#txtAddress").removeAttr('readonly');
        $("#txtState").removeAttr('readonly');
        $("#txtCity").removeAttr('readonly');
        $("#txtZipCode").removeAttr('readonly');
        $("#Permenant").removeAttr("disabled");
    }
}

//restore values
function RestoreValues() {
    $("#txtAddress").val($("#hdnAddress").val());
    $("#txtState").val($("#hdnState").val());
    $("#txtCity").val($("#hdnCity").val());
    $("#txtZipCode").val($("#hdnZipCode").val());
    $("#ddlCountry").val($("#hdCountry").val());
    $("#txtCurrentAddress").val($("#hdnCurrentAddress").val());
    $("#txtCurrentState").val($("#hdnCurrentState").val());
    $("#txtCurrentCity").val($("#hdnCurrentCity").val());
    $("#txtCurrentZipCode").val($("#hdnCurrentZipCode").val());
    $("#ddlCurrentCountry").val($("#hdCurrentCountry").val());
}

function saveResidentialDetails() {
    alert(UserRole);
    // var postUrl = "ResidentialDetails/PersonalDetails";
    //debugger;
    if ($("#frmResident").valid()) {
        $("#loading").dialog({
            closeOnEscape: false,
            resizable: false,
            height: 140,
            width: 300,
            modal: true,
            dialogClass: "noclose"
        });
        if ($('#chkAddress').is(':checked')) {
            var currentadd = $("#txtCurrentAddress").val();
            var currentState = $("#txtCurrentState").val();
            var currentCity = $("#txtCurrentCity").val();
            var currentZipCode = $("#txtCurrentZipCode").val();
            $("#txtAddress").val(currentadd);
            $("#txtState").val(currentState);
            $("#txtCity").val(currentCity);
            $("#txtZipCode").val(currentZipCode);
            $("#ddlCountry")[0].selectedIndex = $("#ddlCurrentCountry")[0].selectedIndex;
        }
        var currentCountry = $('#ddlCurrentCountry').val();
        var country = $('#ddlCountry').val();
        if (country.toString() == 0 || currentCountry.toString() == 0) {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#ResidentialerrorDialog").dialog({
                resizable: false,
                height: 140,
                width: 300,
                modal: true,
                title: 'Personal Details',
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                        return
                    }
                }
            });
        }

        if ($('#UserRole').val() != UserRole) {
            //debugger;
            $("#lblCurrentAddress").val($("#lblcurrentaddress").text());
            $("#lblCurrentCountry").val($("#lblcurrentcountry").text());
            $("#lblCurrentState").val($("#lblcurrentstate").text());
            $("#lblCurrentCity").val($("#lblcurrentcity").text());
            $("#lblCurrentZipCode").val($("#lblcurrentzipcode").text());
            $("#lblAddress").val($("#lbladdress").text());
            $("#lblCountry").val($("#lblcountry").text());
            $("#lblState").val($("#lblstate").text());
            $("#lblCity").val($("#lblcity").text());
            $("#lblZipCode").val($("#lblzipcode").text());
            $.ajax({
                url: "ResidentialChanges/PersonalDetails",
                type: 'POST',
                data: $('#frmResident').serialize(),
                datatype: 'json',
                success: function (result) {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    if (result.label != null) {
                        $.each(result.label, function (i) {
                            if (result.label[i] == $("#lblcurrentaddress").text() && $("#txtCurrentAddress").val() != "") {
                                $("#spCurrentAddress").html('');
                                $("#spCurrentAddress").append(result.approvalMessage);
                                $("#spCurrentAddress").css("color", "red");
                                $("#spCurrentAddress").show();
                                $("#txtCurrentAddress").attr("disabled", "disabled");
                            }
                            if (result.label[i] == $("#lblcurrentcountry").text() && $("#ddlCurrentCountry").val() != "") {
                                $("#spCurrentCountry").html('');
                                $("#spCurrentCountry").append(result.approvalMessage);
                                $("#spCurrentCountry").css("color", "red");
                                $("#spCurrentCountry").show();
                                $("#ddlCurrentCountry").attr("disabled", "disabled");
                            }
                            if (result.label[i] == $("#lblcurrentstate").text() && $("#txtCurrentState").val() != "") {
                                $("#spCurrentState").html('');
                                $("#spCurrentState").append(result.approvalMessage);
                                $("#spCurrentState").css("color", "red");
                                $("#spCurrentState").show();
                                $("#txtCurrentState").attr("disabled", "disabled");
                            }
                            if (result.label[i] == $("#lblcurrentcity").text() && $("#txtCurrentCity").val() != "") {
                                $("#spCurrentCity").html('');
                                $("#spCurrentCity").append(result.approvalMessage);
                                $("#spCurrentCity").css("color", "red");
                                $("#spCurrentCity").show();
                                $("#txtCurrentCity").attr("disabled", "disabled");
                            }
                            if (result.label[i] == $("#lblcurrentzipcode").text() && $("#txtCurrentZipCode").val() != "") {
                                $("#spCurrentZipCode").html('');
                                $("#spCurrentZipCode").append(result.approvalMessage);
                                $("#spCurrentZipCode").css("color", "red");
                                $("#spCurrentZipCode").show();
                                $("#txtCurrentZipCode").attr("disabled", "disabled");
                            }
                            if (result.label[i] == $("#lbladdress").text() && $("#txtAddress").val() != "") {
                                $("#spAddress").html('');
                                $("#spAddress").append(result.approvalMessage);
                                $("#spAddress").css("color", "red");
                                $("#spAddress").show();
                                $("#txtAddress").attr("disabled", "disabled");
                            }
                            if (result.label[i] == $("#lblcountry").text() && $("#ddlCountry").val() != "") {
                                $("#spCountry").html('');
                                $("#spCountry").append(result.approvalMessage);
                                $("#spCountry").css("color", "red");
                                $("#spCountry").show();
                                $("#ddlCountry").attr("disabled", "disabled");
                            }
                            if (result.label[i] == $("#lblstate").text() && $("#txtState").val() != "") {
                                $("#spState").html('');
                                $("#spState").append(result.approvalMessage);
                                $("#spState").css("color", "red");
                                $("#spState").show();
                                $("#txtState").attr("disabled", "disabled");
                            }
                            if (result.label[i] == $("#lblcity").text() && $("#txtCity").val() != "") {
                                $("#spCity").html('');
                                $("#spCity").append(result.approvalMessage);
                                $("#spCity").css("color", "red");
                                $("#spCity").show();
                                $("#txtCity").attr("disabled", "disabled");
                            }
                            if (result.label[i] == $("#lblzipcode").text() && $("#txtZipCode").val() != "") {
                                $("#spZipCode").html('');
                                $("#spZipCode").append(result.approvalMessage);
                                $("#spZipCode").css("color", "red");
                                $("#spZipCode").show();
                                $("#txtZipCode").attr("disabled", "disabled");
                            }
                        }); //end label each
                    } //end if

                    if (result.status == true) {
                        $("#successResiDialog").dialog({
                            closeOnEscape: false,
                            title: 'Residential Details',
                            resizable: false,
                            height: 'auto',
                            width: 300,
                            modal: true,
                            buttons: {
                                Ok: function () {
                                    $(this).dialog('close');
                                    $("#loading").dialog({
                                        closeOnEscape: false,
                                        resizable: false,
                                        height: 140,
                                        width: 300,
                                        modal: true,
                                        dialogClass: "noclose"
                                    });

                                    var MailUrl = "MailTemplate/PersonalDetails";
                                    var Parameter = { employeeId: $("#empID").val() }
                                    $.ajax({
                                        url: MailUrl,
                                        type: 'GET',
                                        data: Parameter,
                                        success: function (data) {
                                            $("#loading").dialog("close");
                                            $("#loading").dialog("destroy");
                                            $("#successResiDialog").dialog('close');

                                            if (data) {
                                                $("#PersonalFeildChangeMailDialog").html(data);
                                                $("#PersonalFeildChangeMailDialog").dialog({
                                                    resizable: false,
                                                    height: 520,
                                                    width: 800,
                                                    modal: true,
                                                    title: "Send Mail",
                                                    close: function () {
                                                        $(this).dialog("close");
                                                    }
                                                });
                                            }

                                            $.validator.unobtrusive.parse($("#MailDetails"));
                                            $('#sendSeparationMail').click(function () {
                                                $("#CCErrorMessage").hide();
                                                $("#ToErrorMessage").hide();
                                                if ($('#MailDetails').valid()) {
                                                    $("#loading").dialog({
                                                        closeOnEscape: false,
                                                        resizable: false,
                                                        height: 140,
                                                        width: 300,
                                                        modal: true,
                                                        dialogClass: "noclose"
                                                    });
                                                    var SendMailUrl = "SendEmail/PersonalDetails";
                                                    $.ajax({
                                                        url: SendMailUrl,
                                                        type: 'POST',
                                                        data: $('#MailDetails').serialize(),
                                                        success: function (data) {
                                                            $("#loading").dialog("close");
                                                            $("#loading").dialog("destroy");
                                                            if (data.validCcId == true && data.validtoId == true) {
                                                                if (data.status == true) {
                                                                    $("#PersonalFeildChangeMailDialog").dialog('close');
                                                                    window.location.reload();
                                                                }
                                                            }
                                                            else if (data.status == "Error") {
                                                                $("#loading").dialog("close");
                                                                $("#loading").dialog("destroy");
                                                                $("#errorDialog").dialog({
                                                                    title: 'Mail Error',
                                                                    resizable: false,
                                                                    height: 'auto',
                                                                    width: 'auto',
                                                                    modal: true,
                                                                    buttons: {
                                                                        Ok: function () {
                                                                            $(this).dialog("close");
                                                                            $("#PersonalFeildChangeMailDialog").dialog('close');
                                                                            window.location.reload();
                                                                        }
                                                                    },
                                                                    close: function () {
                                                                        $(this).dialog("close");
                                                                        window.location.reload();
                                                                    }
                                                                }); //end dialog
                                                            }
                                                            else {
                                                                $("#loading").dialog("close");
                                                                $("#loading").dialog("destroy");
                                                                if (data.validCcId == false)
                                                                    $("#CCErrorMessage").show();

                                                                if (data.validtoId == false)
                                                                    $("#ToErrorMessage").show();

                                                                return false;
                                                            }
                                                        }, //end send success
                                                        error: function () {
                                                            $("#loading").dialog("close");
                                                            $("#loading").dialog("destroy");
                                                            $("#mailError").dialog({
                                                                title: 'Mail Error',
                                                                resizable: false,
                                                                height: 'auto',
                                                                width: 'auto',
                                                                modal: true,
                                                                buttons: {
                                                                    Ok: function () {
                                                                        $(this).dialog("close");
                                                                        $("#PersonalFeildChangeMailDialog").dialog('close');
                                                                        window.location.reload();
                                                                    }
                                                                },
                                                                close: function () {
                                                                    $(this).dialog("close");
                                                                    window.location.reload();
                                                                }
                                                            }); //end dialog
                                                        }
                                                    }); //end send ajax
                                                } //end valid
                                            }); //end click
                                        }, //end sucecss
                                        error: function () {
                                            $("#loading").dialog("close");
                                            $("#loading").dialog("destroy");
                                            $("#errorDialog").dialog({
                                                title: 'Mail Error',
                                                resizable: false,
                                                height: 'auto',
                                                width: 'auto',
                                                modal: true,
                                                buttons: {
                                                    Ok: function () {
                                                        $(this).dialog("close");
                                                        window.location.reload();
                                                    }
                                                }
                                            }); //end dialog
                                        } //end error
                                    }); //end show mail ajax
                                }
                            }
                        });
                    } //end if==true

                    else if (result.status == "Error") {
                        $("#loading").dialog("close");
                        $("#loading").dialog("destroy");
                        $("#errorDialog").dialog({
                            resizable: false,
                            height: 'auto',
                            width: 'auto',
                            modal: true,
                            title: 'Residential Details',
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
                        $("#NoChange").dialog({
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
                }, //end main success
                error: function (result) {
                    $("#loading").dialog("close");
                    $("#loading").dialog("destroy");
                    $("#errorDialog").dialog({
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
            });  //end ajax
        } //end if
        else {
            //debugger;
            if ($('#frmResident').valid()) {
                $("#loading").dialog({
                    closeOnEscape: false,
                    resizable: false,
                    height: 140,
                    width: 300,
                    modal: true,
                    dialogClass: "noclose"
                });
                $.ajax({
                    url: "ResidentialDetails/PersonalDetails",
                    cache: false,
                    type: 'POST',
                    data: $('#frmResident').serialize(),
                    success: function (results) {
                        alert(results);
                        $("#loading").dialog("close");
                        $("#loading").dialog("destroy");
                        if (results.status == true) {
                            $("#successResiDialog").dialog({
                                resizable: false,
                                height: 140,
                                width: 300,
                                modal: true,
                                title: 'Residential Details',
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            });
                        }
                        else if (results.status == "Error") {
                            $("#loading").dialog("close");
                            $("#loading").dialog("destroy");
                            $("#errorDialog").dialog({
                                resizable: false,
                                height: 'auto',
                                width: 'auto',
                                modal: true,
                                title: 'Residential Details',
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
                            $("#errorDialog").dialog({
                                resizable: false,
                                height: 140,
                                width: 300,
                                title: 'Residential Details',
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            });
                        }
                    }
                });
            } //end valid
            else
                return false;
        } //end else
    }
    else
        return false;
}