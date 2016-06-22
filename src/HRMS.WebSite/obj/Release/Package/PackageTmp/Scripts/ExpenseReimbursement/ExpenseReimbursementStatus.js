function FieldDDLChange() {
    if ($("#Field").val() != "") {
        var url = "GetFieldChildDetailsList/ExpenseReimbursement";
        $("#FieldChild").show();
        $.getJSON(url, { Field: $("#Field").val() }, function (data) {
            //Clear the Model list
            $("#FieldChild").empty();
            $("#FieldChild").append("<option value='" + "" + "'>" + "" + "</option>");
            //Foreach Model in the list, add a model option from the data returned
            $.each(data, function (index, optionData) {
                $("#FieldChild").append("<option value='" + optionData.Id + "'>" + optionData.Description + "</option>");
            });
        });
    } else {
        $("#FieldChild").val("");
        $("#FieldChild").hide();
    }
}