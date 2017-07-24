<%@ Page Title="ReportIssue" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master"
    AutoEventWireup="True" CodeBehind="ReportIssue.aspx.cs" Inherits="HRMS.HelpDesk.ReportIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <link href="../Content/New%20Design/helpdesk.css" rel="stylesheet" />
    <script src="Script/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../Scripts/Global.min.js" type="text/javascript"></script>
    <%--By Rahul R:Increasing Width of Dropdowns--%> 
    <style>
        .selectBox-options LI {
            width: 500px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            $(".FileUploadBtn").bind("change", function (event) {
                var path = $(this).val().replace("C:\\fakepath\\", "");
                $("#FileUploadField").val(path);
            });

            var newResource = $('#MainContent_txtNewResource').val();
            var currentAllocation = $('#MainContent_txtUpdateCurrentAllocation').val();
            var bulkExtension = $('#MainContent_txtSingleOrBulkExtension').val();

            $('#MainContent_txtUpdateCurrentAllocation').hide();
            $('#MainContent_txtSingleOrBulkExtension').hide();
            $('#MainContent_txtNewResource').hide();
            //            var e = document.getElementById("ddlSubCategories");
            //            var strUser = e.options[e.selectedIndex].value;
            var strUser = $('#MainContent_ddlSubCategories :selected').val();
            if (strUser == $('#MainContent_txtNewResource').val() || strUser == $('#MainContent_txtUpdateCurrentAllocation').val()) {
                $('.OtherCategoryShow').hide();
                $('#ddltype').val('1');
                $('#ddltype').prop('disabled', true);
                $('.PMSCategories').show();
            }
            else if (strUser == $('#MainContent_txtSingleOrBulkExtension').val()) {
                $('.OtherCategoryShow').hide();
                $('#ddltype').val('1');
                $('#ddltype').prop('disabled', true);
                $('.PMSCategories').hide();
                $('.BulkCategories').show();
            }
            else {
                $('.OtherCategoryShow').show();
                $('.PMSCategories').hide();
                $('#ddltype').prop('disabled', false);
                $('#ddltype').val('');
            }
            $('#MainContent_txtWorkHours').keyup(function () {
                if ($(this).val() > 100) {
                    alert("Enter number between 0 to 100");
                    $(this).val('100');
                }
                Checkone();
            });

            DisplayLabel();
            function DisplayLabel() {
                $.each($(".ClassDisabledFields"), function (l, val) {
                    if ($(val).is(':disabled')) {

                        if (val.id == "MainContent_txtName") {
                            $(this).val();
                            $(this).next().val($(this).val());
                            $('#MainContent_lbltxtName').text($(this).next().val());
                            $(this).hide().next().show();
                        }
                        if (val.id == "MainContent_txtEmailID") {
                            $(this).val();
                            $(this).next().val($(this).val());
                            $('#MainContent_lbltxtEmailID').text($(this).next().val());
                            $(this).hide().next().show();
                        }

                    }
                });
            }

            $("#MainContent_txtDescription").keyup(function () {
                var charLength = 1000 - $(this).val().length;
                $(this).next("span").html(charLength);
            });

        });
        function Checkone() {
            document.getElementById('MainContent_lblError').style = "display:none";
            document.getElementById('MainContent_lblMessage').style = "display:none";
        }
        //function validateAndCheck() {
        //  //  check();
        //    if (validate()) {

        //        if (checkSubCategorySelection()) {
        //            try {

        //                $('#MainContent_ddltype').prop('disabled', false);
        //                DisplayLoadingDialog();
        //                //                        $("#divContent").dialog({
        //                //                            closeOnEscape: false,
        //                //                            resizable: false,
        //                //                            height: 140,
        //                //                            width: 300,
        //                //                            modal: true,
        //                //                            dialogClass: "noclose"
        //                //                        });

        //            }
        //            catch (e) {
        //                $.unblockUI();
        //            }
        //            return true;

        //        }
        //        else return false;
        //    }
        //    else return false;
        //}
        //function validate() {
        //    var i = null;
        //    var category = $('#MainContent_ddlSubCategories').val();
        //    if (category == undefined) {
        //        i = isRequire("MainContent_txtName^MainContent_txtEmailID^MainContent_txtPhoneExtension^MainContent_txtSeatingLocation^MainContent_ddlCategories^MainContent_ddltype^MainContent_txtDescription", "Name^Email ID^Phone Ext^Seating Location^Category^Type^Description", this.enabled)
        //    } else {
        //        if (category == $('#MainContent_txtNewResource').val() || category == $('#MainContent_txtUpdateCurrentAllocation').val())
        //            i = isRequire("MainContent_txtName^MainContent_txtEmailID^MainContent_ddlSubCategories^MainContent_ddlProjectName^MainContent_ddlProjectRole^MainContent_txtWorkHours^MainContent_txtFromDate^MainContent_txtEndDate^MainContent_txtNoOfResources^MainContent_ddlResourcePool^MainContent_ddlReportingTo^MainContent_txtDescription", "Name^Email ID^Sub Category^Project Name^Project Role^Work Hour^From Date^To Date^No of Resource^Resource Pool^Reporting To^Description", this.enabled)
        //        else if (category == $('#MainContent_txtSingleOrBulkExtension').val())
        //            i = isRequire("MainContent_txtName^MainContent_txtEmailID^MainContent_ddlSubCategories^MainContent_ddlProjectName^MainContent_txtEndDate^MainContent_txtNoOfResources^MainContent_txtDescription", "Name^Email ID^Sub Category^Project Name^End Date^No of Resource^Description", this.enabled)
        //        else
        //            i = isRequire("MainContent_txtName^MainContent_txtEmailID^MainContent_txtPhoneExtension^MainContent_txtSeatingLocation^MainContent_ddlSubCategories^MainContent_ddltype^MainContent_txtDescription", "Name^Email ID^Phone Ext^Seating Location^Sub Category^Type^Description", this.enabled)
        //    }
        //    if (i == true) {
        //        var from = new Date($('#MainContent_txtFromDate').val());
        //        var to = new Date($('#MainContent_txtEndDate').val());
        //        if (from > to) {
        //            alert("Start date cannot be greater than end date");
        //            return false;
        //        }
        //        else {
        //            if (validateV2Email("MainContent_txtEmailID", "Email ID") && validateV2Email("MainContent_txtCCEmailID", "CC Email ID")) {

        //                if (category == $('#MainContent_txtNewResource').val() || category == $('#MainContent_txtUpdateCurrentAllocation').val() || category == $('#MainContent_txtSingleOrBulkExtension').val())
        //                    return true;
        //                else if (checkNumeric("MainContent_txtPhoneExtension", "Phone Extension"))
        //                    return true;
        //                else
        //                    return false;
        //            }
        //            else {
        //                return false;
        //            }
        //        }
        //    }
        //    else {
        //        return false;
        //    }
        //}

        //$("#ddlCategories").click()
        //{
        //    alert("hi");
        //    var ddlCategories = document.getElementById("MainContent_ddlCategories");
        //    debugger;
        //    if (ddlCategories.options[ddlCategories.selectedIndex].value == "12") {
        //        $("#subCategoryId").hide();
        //    }

        //    else {
        //        $("#subCategoryId").show();
        //    }
        //}

        function setfocus() {
            document.getElementById("MainContent_txtName").focus();

        }
        function allowOnlyNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            Checkone();
            return true;
        }
        function projectNameChange() {

            var projectId = $('#MainContent_ddlProjectName').val();

            $.ajax({
                type: "POST",
                url: "ReportIssue.aspx/GetProjectDates",
                contentType: 'application/json; charset=utf-8',
                data: "{ 'stringParam': '" + projectId + "'}",
                dataType: "json",
                async: true,
                cache: false,
                success: function (data) {

                }
            });

        }

        // Added By Mahesh F
        // To Avoid HTML Tag in txtDescription
        function htmlNotallowFunction() {
            str = (document.getElementById('MainContent_txtDescription')).value;

            if (str.match(/<("[^"]*"|'[^']*'|[^'">])*>/i) == null) {
                document.getElementById('MainContent_lbltxtError').style = "display:none";
                $('#MainContent_lbltxtError').hide();
            }
            else {
                document.getElementById('MainContent_lbltxtError').innerHTML = "Text Inclosed in <> is not allow"
                $('#MainContent_lbltxtError').show();
            }
        }
        function leaveChange() {
            document.getElementById('MainContent_lblError').style = "display:none";

            //            var e = document.getElementById("ddlCategories");
            //            var strUser = e.options[e.selectedIndex].value;
            var strUser = $('#MainContent_ddlSubCategories :selected').val();
            if (strUser == $('#MainContent_txtNewResource').val() || strUser == $('#MainContent_txtUpdateCurrentAllocation').val()) {
                $('.OtherCategoryShow').hide();
                $('#ddltype').val('1');
                $('#ddltype').prop('disabled', true);
                $('.PMSCategories').show();
            }
            else if (strUser == $('#MainContent_txtSingleOrBulkExtension').val()) {
                $('.OtherCategoryShow').hide();
                $('#ddltype').val('1');
                $('#ddltype').prop('disabled', true);
                $('.PMSCategories').hide();
                $('.BulkCategories').show();
            }
            else {
                $('.OtherCategoryShow').show();
                $('.PMSCategories').hide();
                $('#ddltype').prop('disabled', false);
                $('#ddltype').val('');
            }

            $.ajax({
                type: "post",
                url: 'ReportIssue.aspx/getDropdomdata',
                data: "{ 'stringParam': '" + strUser + "'}",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                async: true,
                success: function (data) {
                    //document.getElementById("lblCategorySummary").innerHTML = data.d.toString();
                    $('#MainContent_lblCategorySummary').text(data.d.toString());
                }
            });    // end $.ajax
        }
    </script>
    <style type="text/css">
        .LoadingWrap {
            margin: auto;
            margin-top: 26px;
        }

        .noclose .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <ajaxToolkit:ToolkitScriptManager EnablePartialRendering="false" runat="Server" ID="ToolkitScriptManager1" />
    <section class="HelpdeskContainer Container">
        <div class="FixedHeader">
            <div class="clearfix">
                <h2 class="MainHeading">HelpDesk</h2>
            </div>
            <nav class="sub-menu-colored">
                <a href="ReportIssue.aspx" class="selected">Report Issue</a> <a href="ViewMyStatus.aspx">Issue Status</a>
            </nav>
        </div>
        <div onload="setfocus();" ms_positioning="GridLayout">
            <div id="body1" runat="server">
                <div class="MainBody HelpdeskMainbody">
                    <div class="FormContainerBox  Helpdesk clearfix">
                        <div style="text-align:center;">
                            <asp:Label ID="DisplayText" Text="Please log all Helpdesk Tickets in VibrantWeb Refresh except for RMG Department" runat="server"  Font-Bold="true" Font-Size="Large" ForeColor="Red"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="lbltxtError" CssClass="error" runat="server"></asp:Label>
                            <asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label>
                            <asp:Label ID="lblMessage" CssClass="success" runat="server"></asp:Label>
                            <asp:Label ID="lblMailError" runat="server" CssClass="error"></asp:Label>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol clearfix">
                                <div class="LabelDiv">
                                    <span class="hiddenstar">*</span>
                                    <label>Name:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtName" CssClass="txtfield ClassDisabledFields" runat="server"
                                        size="50" ValidationGroup="Add" CausesValidation="True" Enabled="False"></asp:TextBox>
                                    <asp:Label ID="lbltxtName" CssClass="ClassDisplayLabel" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="rightcol">
                                <div class="LabelDiv">
                                    <label>
                                        <span class="hiddenstar">*</span> CC to (Manager/Lead Mail ID):</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtCCEmailID" CssClass="txtfield" runat="server" size="50"></asp:TextBox>
                                    <asp:Label ID="lblCcEmailError" CssClass="error" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol clearfix">
                                <div class="LabelDiv">
                                    <span class="hiddenstar">*</span>
                                    <label>Email ID:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtEmailID" CssClass="txtfield ClassDisabledFields" runat="server"
                                        size="50" Enabled="False"></asp:TextBox>
                                    <asp:Label ID="lbltxtEmailID" CssClass="ClassDisplayLabel" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="rightcol clearfix OtherCategoryShow" id="phoneExtension">
                                <div class="LabelDiv">
                                    <label>
                                        * Phone Extension:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtPhoneExtension" onkeypress="Checkone()" CssClass="txtfield" MaxLength="10" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol">
                                <div class="LabelDiv">
                                    <label>
                                        * Categories:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddlCategories" AutoPostBack="true" onchange="Checkone()" runat="server" CssClass="dropdown"
                                        OnSelectedIndexChanged="ddlCategories_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="rightcol OtherCategoryShow" id="seatingLocation">
                                <div class="LabelDiv">
                                    <label>
                                        * Seating Location:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtSeatingLocation" onkeypress="Checkone()" CssClass="txtfield" MaxLength="25" runat="server"></asp:TextBox>&nbsp;
                                    <span class="FormNote">(The code at the desk. Eg: V2MUM/GRFLR/0123)</span>
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix" id="subCategoryId" runat="server" display="none">
                            <div class="leftcol">
                                <div class="LabelDiv">
                                    <label>
                                        * SubCategories:</label>
                                </div>
                                <div class="InputDiv">
                                    <select id="ddlSubCategories" class="dropdown" runat="server" name="ddlSubCategories"
                                        onchange="leaveChange()">
                                    </select>
                                    <asp:Label ID="lblCategorySummary" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix" style="display:none;">
                            <div class="leftcol clearfix">
                                <div class="LabelDiv">
                                    <label>
                                        * Type:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddltype" onchange="Checkone()" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="rightcol" id="sev">
                                <div class="LabelDiv">
                                    <label runat="server" id="severity">
                                        * Severity:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddlSeverity" onchange="Checkone()" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol clearfix" id="uploadFileForIssue">
                                <div class="LabelDiv">
                                    <span class="hiddenstar">*</span>
                                    <label>
                                        Upload File:
                                    </label>
                                </div>
                                <div class="InputDiv positionR BrowseSpacingFix">
                                    <input type="file" id="uploadFiles" class="FileUploadBtn" name="uploadFiles" runat="server"
                                        style="width: 100px" />
                                    <div class="BrowserVisible">
                                        <input type="button" class="BtnForCustomUpload" value="Browse.." /><input type="text"
                                            id="FileUploadField" class="FileField" value="No files selected" />
                                    </div>
                                    <%--<input class="" id="uploadFiles" type="file" style="width: 200px;" size="40" name="uploadFiles"
                                        runat="server">--%>
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol clearfix PMSCategories BulkCategories" id="projectName">
                                <div class="LabelDiv">
                                    <label>
                                        * Project Name:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddlProjectName" runat="server" AutoPostBack="true" EnableViewState="true"
                                        CssClass="dropdown" onchange="projectNameChange()" OnSelectedIndexChanged="ddlProjectName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="rightcol PMSCategories" id="projectRole">
                                <div class="LabelDiv">
                                    <label>
                                        * Project Role:
                                    </label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddlProjectRole" onchange="Checkone()" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol clearfix PMSCategories" id="workHours">
                                <div class="LabelDiv">
                                    <label>
                                        * Work Hour(% of day):</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtWorkHours" CssClass="txtfield" MaxLength="3" runat="server" onkeypress="return allowOnlyNumber(event);"></asp:TextBox>
                                </div>
                            </div>
                            <div class="rightcol PMSCategories BulkCategories" id="noOfResources">
                                <div class="LabelDiv">
                                    <label>
                                        *Count Of Resources:
                                    </label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtNoOfResources" CssClass="txtfield" MaxLength="3" runat="server"
                                        onkeypress="return allowOnlyNumber(event);"></asp:TextBox>&nbsp; <span class="FormNote">(Only numbers are allowed)</span>
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol clearfix PMSCategories" id="fromDate">
                                <div class="LabelDiv">
                                    <label>
                                        * From Date:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="hasDatepicker" ValidationGroup="ADD"></asp:TextBox>
                                    <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server"
                                        ID="imgbtnFromDate" ImageAlign="AbsMiddle" CausesValidation="false" CssClass="ui-datepicker-trigger" />
                                    <ajaxToolkit:CalendarExtender ID="calFromDate" runat="server" TargetControlID="txtFromDate"
                                        PopupButtonID="imgbtnFromDate" />
                                </div>
                            </div>
                            <div class="rightcol PMSCategories BulkCategories" id="endDate">
                                <div class="LabelDiv">
                                    <label>
                                        * End Date:
                                    </label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtEndDate" onchange="Checkone()" runat="server" CssClass="hasDatepicker" ValidationGroup="ADD"></asp:TextBox>
                                    <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server"
                                        ID="imgbtnEndDate" ImageAlign="AbsMiddle" CausesValidation="false" CssClass="ui-datepicker-trigger" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate"
                                        PopupButtonID="imgbtnEndDate" />
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol clearfix PMSCategories" id="resourcePool">
                                <div class="LabelDiv">
                                    <label>
                                        * Resource Pool:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddlResourcePool" onchange="Checkone()" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="rightcol PMSCategories" id="reportingTo">
                                <div class="LabelDiv">
                                    <label>
                                        * Will be Reporting to:
                                    </label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddlReportingTo" onchange="Checkone()" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol clearfix">
                                <div class="LabelDiv">
                                    <label>
                                        * Description(Resource Names with Billable status/Any other comments over the specific action to be taken.):</label>
                                </div>
                                <div class="InputDiv">
                                    <textarea class="txtfield" id="txtDescription" onkeydown="textCounter(txtDescription,txtDescCount,1000)"
                                        onkeypress="Checkone()" onkeyup="textCounter(txtDescription,txtDescCount,1000)" name="txtDescription" maxlength="1000" onblur="htmlNotallowFunction()"
                                        runat="server"></textarea>
                                    <%--<input class="txtfieldlimit" id="txtDescCount" readonly type="text" maxlength="3"
                                        size="3" value="1000" name="txtDescCount">--%>

                                    <span>1000</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="fill-dtls clearfix">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="Add" CausesValidation="True"
                            class="ButtonGray" ></asp:Button>
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" class="ButtonGray" ></asp:Button>
                    </div>
                    <div>
                        <asp:TextBox ID="txtUpdateCurrentAllocation" hidden="true" runat="server" Text="<%$appSettings:UpdateCurrentAllocation %>" />
                        <asp:TextBox ID="txtNewResource" hidden="true" runat="server" Text="<%$appSettings:NewResource %>" />
                        <asp:TextBox ID="txtSingleOrBulkExtension" hidden="true" runat="server" Text="<%$appSettings:SingleOrBulkExtension %>" />
                    </div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="Add" />
                </div>
            </div>
            <div id="divContent" title="Please Wait..." style="display: none">
                <p class="LoadingWrap" style="width: 25px;">
                    <img src="Images/Loading.gif" style="width: 40px; height: 40px;" alt="Loading..." />
                </p>
            </div>
            <div id="startDateRequired" title="Error" style="display: none">
                Please Select Start Date.
            </div>
            <div id="endDateRequired" title="Error" style="display: none">
                Please Select End Date.
            </div>
        </div>
    </section>

    <script type="text/javascript" language="javascript" src="footer.js"></script>
    <script type="text/javascript" language="javascript" src="js/common.js"></script>
</asp:Content>