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
 function HRSendMail() {
	var counter = 0;
	$('.case').each(function () {
		if ($(this).is(':checked'))
			counter = 1;
	});

	if (counter == 1) {
	    DisplayLoadingDialog(); //checked

		var collection = "";
		$.each($("input:checked"), function (i, val) {
			if ($(val).attr("data-name") != undefined)
				collection = collection + $(val).attr("data-name") + ";";
		});
		var MailUrl = "PendingClearanceMailTemplate/Exit";
		var Parameter = { employeeId: EmpIdToPassHR }
		$.ajax({
			url: MailUrl,
			type: 'GET',
			cache: false,
			data: { "employeeId": EmpIdToPassHR, "collection": collection },
			contentType: 'application/json; charset=utf-8',
			success: function (data) {
				$("#loading").dialog("close");
				$("#loading").dialog("destroy");

				if (data) {
					$("#SeparationMailDialog").html(data);
					$("#SeparationMailDialog").dialog({
						resizable: false,
						height: 'auto',
						width: 800,
						modal: true,
						title: "Send Mail",
						open: function () {
						    $(this).parent().prev('.ui-widget-overlay').css('z-index', '30');
						    $(this).parent().css('z-index', '31');
						},
						close: function () {
							$(this).dialog('close');
						}
					});
					$.validator.unobtrusive.parse($("#MailDetails"));
					$('#sendSeparationMail').click(function () {
						$("#CCErrorMessage").hide();
						$("#ToErrorMessage").hide();
						if ($('#MailDetails').valid()) {
						    DisplayLoadingDialog();  //checked
							var SendMailUrl = "SendEmail/Exit";
							$.ajax({
								url: SendMailUrl,
								type: 'POST',
								cache: false,
								data: $('#MailDetails').serialize(),
								success: function (data) {
									$("#loading").dialog("close");
									$("#loading").dialog("destroy");
									//$("#SeparationMailDialog").dialog('destroy');
									//$('#btnHRSendPendingItem').dialog('destroy');
									if (data.validCcId == true && data.validtoId == true) {
										if (data.status == true) {
											$("#SeparationMailDialog").dialog('destroy');
											$('#btnHRSendPendingItem').dialog('destroy');
										}
									}
									else if (data.status == "Error") {
										$("#errorDialog").dialog({
											title: 'Mail Error',
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
										if (data.validCcId == false)
											$("#CCErrorMessage").show();

										if (data.validtoId == false)
											$("#ToErrorMessage").show();

										return false;
									}
								},
								error: function () {
									$("#loading").dialog("close");
									$("#loading").dialog("destroy");
									$("#errorDialog").dialog({
										title: 'Mail Error',
										resizable: false,
										height: 'auto',
										width: 'auto',
										modal: true,
										buttons:
                                                   {
                                                   	Ok: function () {
                                                   		$(this).dialog("close");
                                                   	}
                                                   }
									}); //end dialog
									//window.location.href = '@@Url.Action("EmpSeparationApprovals", "Exit")';
								}
							}); //end ajax
						}
					});
				}
			}
		});
	}
	else {
		$("#ChkBoxWarningHR").dialog({
			title: 'Pending Item',
			resizable: false,
			height: 'auto',
			width: 'auto',
			modal: true,
			open: function () {
			    $(this).parent().prev('.ui-widget-overlay').css('z-index', '28');
			    $(this).parent().css('z-index', '29');
			},
			buttons:
            {
            	Ok: function () {
            		$(this).dialog("close");
            	}
            }
		}); //end dialog
	}
}