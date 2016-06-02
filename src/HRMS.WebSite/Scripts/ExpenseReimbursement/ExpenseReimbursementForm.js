function callExpenseReimursementMail(expenseID, isaprroved, isrejected, isCancelled, employeeID, stageID, primaryApprover, secondaryApprover, FinanceApprover, Formname, comments, ExpenseCode) {
    $.ajax({
        url: "/ExpenseReimbursement/ExpenseReimbursementMail",
        data: { expenseId: expenseID, isApproveCall: isaprroved, IsRejectCall: isrejected, IsCancelled: isCancelled, employeeID: employeeID, stageID: stageID, primaryApprover: primaryApprover, secondaryApprover: secondaryApprover, FinanceApprover: FinanceApprover, Formname: Formname, comments: comments, ExpenseCode: ExpenseCode },
        type: 'POST',
        async: false,
        cache: false,
        success: function (data) {
                        $("#loading").dialog("close");
                        $("#loading").dialog("destroy");
            //            $("#CancelAllExpDelDialog").dialog("destroy");
            //            $("#RejectConfirmationMessage").dialog("destroy");

            if (data.status == true) {
                if (isCancelled == true) {
                    $("#mailSendSuccess").dialog({
                        height: 140,
                        width: 300,
                        modal: true,
                        title: "Expense Details",
                        open: function (event, ui) {
                            setTimeout("$('#mailSendSuccess').dialog('close')", 1000);
                        }
                    });
                    window.location.href = "/ExpenseReimbursement/GetExpenseReimbursementStatus";
                }
                if (isaprroved == true) {
                    $("#SubmitSuccessMessage").dialog({
                        height: 140,
                        width: 300,
                        modal: true,
                        title: "Expense Details",
                        open: function (event, ui) {
                            setTimeout("$('#SubmitSuccessMessage').dialog('close')", 1000);
                        }
                    });
                    window.location.href = "/ExpenseReimbursement/GetExpenseReimbursementStatus";
                }
                else if (isrejected == true) {
                    $("#RejectSuccessMessage").dialog({
                        height: 140,
                        width: 300,
                        modal: true,
                        title: "Expense Details",
                        open: function (event, ui) {
                            setTimeout("$('#RejectSuccessMessage').dialog('close')", 1000);
                        }
                    });
                    window.location.href = "/ExpenseReimbursement/GetExpenseReimbursementStatus";
                }
            }
            else if (data.status == "Error") {
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 300,
                    modal: true,
                    title: 'Mail Error',
                    dialogClass: 'noclose',
                    open: function (event, ui) {
                        setTimeout("$('#errorDialog').dialog('close')", 1000);
                    }
                    //buttons: {
                    //    Ok: function () {
                    //        $("#SubmitSuccessMessage").dialog({
                    //            height: 140,
                    //            width: 300,
                    //            modal: true,
                    //            title: "Expense Details",
                    //            open: function (event, ui) {
                    //                setTimeout("$('#SubmitSuccessMessage').dialog('close')", 1000);
                    //            }
                    //        });
                    //        window.location.href = "/ExpenseReimbursement/GetExpenseReimbursementStatus";
                    //        $(this).dialog("close");
                    //    }
                    //}
                }); //end dialog
                window.location.href = "/ExpenseReimbursement/GetExpenseReimbursementStatus";
                $(this).dialog("close");
            }
            else if (data.status == "ErrorRecipient") {
                $("#failedRecipient #span_failedRecipient").append(data.failedRecipient);

                $("#SubmitSuccessMessage").dialog({
                    height: 140,
                    width: 300,
                    modal: true,
                    title: "Expense Details",
                    open: function (event, ui) {
                        setTimeout("$('#SubmitSuccessMessage').dialog('close')", 1000);
                    }
                });
                window.location.href = "/ExpenseReimbursement/GetExpenseReimbursementStatus";

                $("#failedRecipient").dialog({
                    closeOnEscape: false,
                    resizable: false,
                    height: 'auto',
                    width: 300,
                    modal: true,
                    dialogClass: 'noclose',
                    buttons: {
                        Ok: function () {
                            window.location.href = "/ExpenseReimbursement/GetExpenseReimbursementStatus";
                            $(this).dialog('close');
                        }
                    }
                });
            }
            else {
                $("#errorDialog").dialog({
                    resizable: false,
                    height: 'auto',
                    width: 'auto',
                    width: 300,
                    modal: true,
                    title: 'Mail Error',
                    dialogClass: 'noclose',
                    buttons: {
                        Ok: function () {
                            window.location.href = "/ExpenseReimbursement/GetExpenseReimbursementStatus";
                            $(this).dialog("close");
                            // window.location.href = "/ExpenseReimbursement/GetExpenseReimbursementStatus";
                        }
                    }
                }); //end dialog
            }
        },
        error: function () {
            $("#loading").dialog("close");
            $("#loading").dialog("destroy");
            $("#mailError").dialog({
                resizable: false,
                height: 'auto',
                width: 300,
                modal: true,
                dialogClass: 'noclose',
                title: 'Mail Error',
                buttons: {
                    Ok: function () {
                        window.location.href = "/ExpenseReimbursement/GetExpenseReimbursementStatus";
                        $(this).dialog("close");
                        // window.location.href = "/ExpenseReimbursement/GetExpenseReimbursementStatus";
                    }
                }
            });
        }
    });
}