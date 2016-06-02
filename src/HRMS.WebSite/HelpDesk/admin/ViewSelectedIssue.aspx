<%@ Page Language="c#" CodeBehind="ViewSelectedIssue.aspx.cs" AutoEventWireup="True"
    Inherits="V2.Helpdesk.web.admin.ViewSelectedIssue" ValidateRequest="false" MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
    <script language="javascript">
        //added by nikhil
        $(document).ready(function () {
            $('#MainContent_hdnStatusOfIssue').hide();
            $('#MainContent_lknNewAllocation').click(function () {

                var helpdeskid1 = $('#MainContent_lblIssueID').text();
                var projectid1 = $('#MainContent_hdnProjectNameId').val();

                var projectrole1 = $('#MainContent_lblProjectRole').text();
                var txtWorkHours1 = $('#MainContent_txtWorkHours').val();
                var txtFromDate1 = $('#MainContent_txtFromDate').val();
                var txtEndDate1 = $('#MainContent_txtEndDate').val();
                var hdnReportedByEmpId1 = $('#MainContent_hdnReportedByEmpId').val();

                $.ajax({
                    type: "POST",
                    url: 'ViewSelectedIssue.aspx/GetNewAllocation',
                    data: "{ 'helpdeskid': '" + helpdeskid1 + " ','projectid': '" + projectid1 + " ','projectrole' : '" + projectrole1 + " ','workhours': '" + txtWorkHours1 + " ','fromdate' : '" + txtFromDate1 + " ','enddate': '" + txtEndDate1 + " ','hdnreportedbyEmpid' : '" + hdnReportedByEmpId1 + "'}",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (result) {
                        var helpdeskid2 = "";
                        var projectid2 = "";
                        var projectrole2 = "";
                        var txtWorkHours2 = ""
                        var txtFromDate2 = ""
                        var txtEndDate2 = "";
                        var hdnReportedByEmpId2 = ""
                        var Temp = result.d.replace('[', '').replace(']', '');
                        var Results = [];
                        Results = Temp.split(',');
                        helpdeskid2 = Results[0].replace('\"', '');;
                        projectid2 = Results[1].replace('\"', '');;
                        projectrole2 = Results[2].replace('\"', '');;
                        txtWorkHours2 = Results[3].replace('\"', '');;
                        txtFromDate2 = Results[4].replace('\"', '');;
                        txtEndDate2 = Results[5].replace('\"', '');;
                        hdnReportedByEmpId2 = Results[6].replace('\"', '');
                        var url1 = "http://" + window.location.host + "/Resource/AddEditResourse?HelpdeskTicketIDs=" + helpdeskid2 + "&ProjectIds=" + projectid2 + "&ProjectRoles=" + projectrole2 + "&WorkHourss=" + txtWorkHours2 + "&FromDates=" + txtFromDate2 + "&ToDates=" + txtEndDate2 + "&Mode=NewAllocation" + "&ReportedByIds=" + hdnReportedByEmpId2;
                        window.parent.open(url1, "_blank");
                    }

                    //var url = "http://" + window.location.host + '/Resource/AddEditResourse?HelpdeskTicketID=' + $('#MainContent_lblIssueID').text() + '&ProjectId=' + $('#MainContent_hdnProjectNameId').text() + '&ProjectRole=' + $('#MainContent_lblProjectRole').text() + '&WorkHours=' + $('#MainContent_txtWorkHours').val() + '&FromDate=' + $('#MainContent_txtFromDate').val() + '&ToDate=' + $('#MainContent_txtEndDate').val() + '&Mode=' + 'NewAllocation' + '&ReportedById=' + $('#MainContent_hdnReportedByEmpId').val();
                    //window.parent.open(url, "_blank");
                });
            });
            $('#MainContent_lnkUpdateCurrentAllocation').click(function () {

                var helpdeskid1 = $('#MainContent_lblIssueID').text();
                var projectid1 = $('#MainContent_hdnProjectNameId').val();

                var projectrole1 = $('#MainContent_lblProjectRole').text();
                var txtWorkHours1 = $('#MainContent_txtWorkHours').val();
                var txtFromDate1 = $('#MainContent_txtFromDate').val();
                var txtEndDate1 = $('#MainContent_txtEndDate').val();
                var hdnReportedByEmpId1 = $('#MainContent_hdnReportedByEmpId').val();

                $.ajax({
                    type: "POST",
                    url: 'ViewSelectedIssue.aspx/GetUpdateCurrentAllocation',
                    data: "{ 'helpdeskid': '" + helpdeskid1 + " ','projectid': '" + projectid1 + " ','projectrole' : '" + projectrole1 + " ','workhours': '" + txtWorkHours1 + " ','fromdate' : '" + txtFromDate1 + " ','enddate': '" + txtEndDate1 + " ','hdnreportedbyEmpid' : '" + hdnReportedByEmpId1 + "'}",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (result) {
                        var helpdeskid2 = "";
                        var projectid2 = "";
                        var projectrole2 = "";
                        var txtWorkHours2 = ""
                        var txtFromDate2 = ""
                        var txtEndDate2 = "";
                        var hdnReportedByEmpId2 = ""
                        var Temp = result.d.replace('[', '').replace(']', '');
                        var Results = [];
                        Results = Temp.split(',');
                        helpdeskid2 = Results[0].replace('\"', '');
                        projectid2 = Results[1].replace('\"', '');
                        projectrole2 = Results[2].replace('\"', '');
                        txtWorkHours2 = Results[3].replace('\"', '');
                        txtFromDate2 = Results[4].replace('\"', '');
                        txtEndDate2 = Results[5].replace('\"', '');
                        hdnReportedByEmpId2 = Results[6].replace('\"', '');
                        var url1 = "http://" + window.location.host + "/Resource/AddEditResourse?HelpdeskTicketIDs=" + helpdeskid2.replace('\"', '') + "&ProjectIds=" + projectid2.replace('\"', '') + "&ProjectRoles=" + projectrole2.replace('\"', '') + "&WorkHourss=" + txtWorkHours2.replace('\"', '') + "&FromDates=" + txtFromDate2.replace('\"', '') + "&ToDates=" + txtEndDate2.replace('\"', '') + "&Mode=UpdateAllocation" + "&ReportedByIds=" + hdnReportedByEmpId2.replace('\"', '');
                        window.parent.open(url1, "_blank");

                    }
                });

                //var url = "http://" + window.location.host + '/Resource/AddEditResourse?HelpdeskTicketID=' + $('#MainContent_lblIssueID').text() + '&ProjectId=' + $('#MainContent_hdnProjectNameId').attr('value') + '&ProjectRole=' + $('#MainContent_lblProjectRole').text() + '&WorkHours=' + $('#MainContent_txtWorkHours').val() + '&FromDate=' + $('#MainContent_txtFromDate').val() + '&ToDate=' + $('#MainContent_txtEndDate').val() + '&Mode=' + 'UpdateAllocation' + '&ReportedById=' + $('#MainContent_hdnReportedByEmpId').val();
                //window.parent.open(url, "_blank");
            });
            $('#MainContent_lnkReallocation').click(function () {

                var helpdeskid1 = $('#MainContent_lblIssueID').text();
                var projectid1 = $('#MainContent_hdnProjectNameId').val();
                var hdnreportedbyEmpid1 = $('#MainContent_hdnReportedByEmpId').val();
                $.ajax({
                    type: "POST",
                    url: 'ViewSelectedIssue.aspx/GetIssueDetails',
                    data: "{ 'helpdeskid': '" + helpdeskid1 + " ','projectid': '" + projectid1 + " ','hdnreportedbyEmpid' : '" + hdnreportedbyEmpid1 + "'}",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (result) {
                        var helpdeskid2 = "";
                        var projectid2 = "";
                        var hdnreportedbyEmpid2 = "";

                        var Temp = result.d.replace('[', '').replace(']', '');
                        var Results = [];
                        Results = Temp.split(',');
                        helpdeskid2 = Results[0];
                        projectid2 = Results[1];
                        hdnreportedbyEmpid2 = Results[2];
                        var url1 = "http://" + window.location.host + '/Resource/ReallocationAndBulkReallocation?HelpdeskTicketIDs=' + helpdeskid2 + '&ProjectIds=' + projectid2 + '&ReportedByIds=' + hdnreportedbyEmpid2;
                        window.parent.open(url1, "_blank");
                    }
                });

                //var url = "http://" + window.location.host + '/Resource/ReallocationAndBulkReallocation?HelpdeskTicketID=' + $('#MainContent_lblIssueID').text() + '&ProjectId=' + $('#MainContent_hdnProjectNameId').attr('value') + '&ReportedById=' + $('#MainContent_hdnReportedByEmpId').val();
                //window.parent.open(url, "_blank");
            });
            $('.RMGHidden').hide();
            var strUser = $('#MainContent_hdnTxtCategory').val();
            if (strUser == $('#MainContent_txtNewResource').val() || strUser == $('#MainContent_txtUpdateCurrentAllocation').val()) {
                $('.OtherCategoryShow').hide();
                $('#ddlIssueType').val('1');
                $('#ddlIssueType').prop('disabled', true);
                $('.PMSCategories').show();
                if (strUser == $('#MainContent_txtNewResource').val()) {
                    $('#MainContent_lknNewAllocation').val('New Allocation');
                    $('.clsAllocationLink').show();
                    $('.clsReAllocationLink').hide();
                    $('.clsUpdateAllocationLink').hide();
                }
                else {
                    $('#MainContent_lnkUpdateCurrentAllocation').text('Update Current Allocation');
                    $('.clsUpdateAllocationLink').show();
                    $('.clsAllocationLink').hide();
                    $('.clsReAllocationLink').hide();
                }
            }
            else if (strUser == $('#MainContent_txtSingleOrBulkExtension').val()) {
                $('.OtherCategoryShow').hide();
                $('#ddlIssueType').val('1');
                $('#ddlIssueType').prop('disabled', true);
                $('.PMSCategories').hide();
                $('.BulkCategories').show();
                $('#lnkReallocation').text('Reallocation');
                $('.clsReAllocationLink').show();
                $('.clsAllocationLink').hide();
                $('.clsUpdateAllocationLink').hide();
            }
            else {
                $('.OtherCategoryShow').show();
                $('.PMSCategories').hide();
                $('#ddlIssueType').prop('disabled', false);
                $('#ddlIssueType').val('');
                $('.clsAllocationLink').hide();
                $('.clsReAllocationLink').hide();
                $('.clsUpdateAllocationLink').hide();
            }
            if ($('#MainContent_hdnStatusOfIssue').text() == 'Closed') {
                $('#MainContent_ddlLoginUser,#MainContent_ddlStatus,#MainContent_txtFromDate,#MainContent_txtEndDate,#MainContent_txtNoOfResources,#MainContent_txtWorkHours').prop('disabled', true);
                $('#MainContent_imgbtnFromDate,#MainContent_imgbtnEndDate,.clsAllocationLink').hide();
            }
            $('#MainContent_txtWorkHours').keyup(function () {
                if ($(this).val() > 100) {
                    alert("Enter number between 0 to 100");
                    $(this).val('100');
                }
            });
            $('#MainContent_btnSubmit').click(function () {
                var startDate = new Date($('#MainContent_txtFromDate').val());
                var endDate = new Date($('#MainContent_txtEndDate').val());
                var projectStartDate = new Date($('#MainContent_hdnFromDate').val());
                var projectEndDate = new Date($('#MainContent_hdnToDate').val());
                var strUser = $('#MainContent_hdnTxtCategory').val();

                if (strUser == $('#MainContent_txtNewResource').val() || strUser == $('#MainContent_txtUpdateCurrentAllocation').val()) {
                    if ($('#MainContent_hdnToDate').val() == "") {
                        $('#MainContent_lblMsg').text("Please set project end date");
                        $('#MainContent_lblMsg').show();
                        return false;
                    }
                    if ($('#MainContent_txtWorkHours').val() == "") {
                        $('#MainContent_lblMsg').text("Please enter work hours");
                        $('#MainContent_lblMsg').show();
                        return false;
                    }
                    if ($('#MainContent_txtFromDate').val() == "") {
                        $('#MainContent_lblMsg').text("Please select start date");
                        $('#MainContent_lblMsg').show();
                        return false;
                    }
                    if ($('#MainContent_txtEndDate').val() == "") {
                        $('#MainContent_lblMsg').text("Please select end date");
                        $('#MainContent_lblMsg').show();
                        return false;
                    }
                    if ($('#MainContent_txtNoOfResources').val() == "") {
                        $('#MainContent_lblMsg').text("Please enter number of sources");
                        $('#MainContent_lblMsg').show();
                        return false;
                    }
                    if (startDate >= projectStartDate && startDate <= projectEndDate
                && endDate >= projectStartDate && endDate <= projectEndDate) {
                        if (endDate > startDate)
                            return true;
                        else {
                            $('#MainContent_lblMsg').text("Start date cannot be greater than end date");
                            $('#MainContent_lblMsg').show();
                            return false;
                        }
                    }
                    else {
                        $('#MainContent_lblMsg').text("Project start date is '" + $('#MainContent_hdnFromDate').val() + "' and Project end date is '" + $('#MainContent_hdnToDate').val() + "'.");
                        $('#MainContent_lblMsg').show();
                        return false;
                    }
                }
                else if (strUser == $('#MainContent_txtSingleOrBulkExtension').val()) {
                    if ($('#MainContent_hdnToDate').val() == "") {
                        $('#MainContent_lblMsg').text("Please set project end date");
                        $('#MainContent_lblMsg').show();
                        return false;
                    }
                    if ($('#MainContent_txtEndDate').val() == "") {
                        $('#MainContent_lblMsg').text("Please select end date");
                        $('#MainContent_lblMsg').show();
                        return false;
                    }
                    if ($('#MainContent_txtNoOfResources').val() == "") {
                        $('#MainContent_lblMsg').text("Please enter number of sources");
                        $('#MainContent_lblMsg').show();
                        return false;
                    }
                    if (endDate >= projectStartDate && endDate <= projectEndDate) {
                        return true;
                    }
                    else {
                        $('#MainContent_lblMsg').text("Project start date is '" + $('#MainContent_hdnFromDate').val() + "' and Project end date is '" + $('#MainContent_hdnToDate').val() + "'.");
                        $('#MainContent_lblMsg').show();
                        return false;
                    }
                }
            });
        });
        //added code end
        function setfocus() {
            document.getElementById("MainContent_txtCause").focus();
        }
        function validate() {
            var ddlstatus = document.getElementById("MainContent_ddlStatus");
            var cause = document.getElementById("MainContent_txtCause");
            var fix = document.getElementById("MainContent_txtFix");
            //            if (ddlstatus.selectedIndex == "2" || ddlstatus.selectedIndex == "3") {
            //                if (cause.value == "") {
            //                    alert("Please enter the Cause");
            //                    return false;
            //                }
            //                if (fix.value == "") {
            //                    alert("Please enter the fix");
            //                    return false;
            //                }
            //            }
        }
        function checkTextAreaMaxLength(textBox, e, length) {

            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!checkSpecialKeys(e)) {
                if (textBox.value.length > maxLength - 1) {
                    if (window.event)//IE
                    {
                        e.returnValue = false;
                        return false;
                    }
                    else//Firefox
                        e.preventDefault();
                }
            }
        }
        function allowOnlyNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function checkSpecialKeys(e) {
            if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 35 && e.keyCode != 36 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                return false;
            else
                return true;
        }
    </script>
    <!-- Loading Calendar JavaScript files -->
    <script src="../utils/zapatec.js" type="text/javascript"></script>
    <script src="../src/calendar.js" type="text/javascript"></script>
    <%-- new files--%>
    <link href="../../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/Global.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/New%20Design/common.js" type="text/javascript"></script>
    <section class="HelpdeskContainer Container">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div class="MainBody">
            <ajaxToolkit:ToolkitScriptManager EnablePartialRendering="true" runat="Server" ID="ScriptManager1" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            </asp:UpdatePanel>
            <asp:Panel ID="pnlIssueDetails" runat="server">
                <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="#ff0033"></asp:Label>
                <div class="FormContainerBox ViewSelected">
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>Issue ID:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblIssueID" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    Issue Reported By:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblIssueReportedBy" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>Issue Reported On:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblIssueReportedOn" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    Phone Extension:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblPhoneExtension" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>Seating Location:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblSeatingLocation" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    Category:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblProblemType" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>Issue Severity:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblProblemSeverity" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    Issue Type:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblIssueType" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>File Name:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Panel ID="pnlFileName" runat="server">
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="rightcol PMSCategories BulkCategories">
                            <div class="LabelDiv">
                                <label>
                                    Project Name:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblProjectName" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix PMSCategories">
                            <div class="LabelDiv">
                                <label>Project Role:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblProjectRole" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="rightcol PMSCategories" id="workHours">
                            <div class="LabelDiv">
                                <label>
                                    Work Hour(% of day):
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtWorkHours" CssClass="txtfield" MaxLength="3" runat="server" onkeypress="return allowOnlyNumber(event);"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix PMSCategories" id="fromDate">
                            <div class="LabelDiv">
                                <asp:Label ID="lblFromDate" runat="server" Text="From Date:"></asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtfield" ValidationGroup="ADD" Width="203px"></asp:TextBox>
                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" CssClass="ui-datepicker-trigger" runat="server" ID="imgbtnFromDate"
                                    ImageAlign="AbsMiddle" CausesValidation="false" />
                                <ajaxToolkit:CalendarExtender ID="calFromDate" runat="server" TargetControlID="txtFromDate"
                                    Format="MM/dd/yyyy" PopupButtonID="imgbtnFromDate" />
                            </div>
                        </div>
                        <div class="rightcol PMSCategories BulkCategories" id="endDate">
                            <div class="LabelDiv">
                                <asp:Label ID="lblEndDate" runat="server" Text="End Date:"></asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="txtfield" ValidationGroup="ADD" Width="203px"></asp:TextBox>
                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" CssClass="ui-datepicker-trigger" runat="server" ID="imgbtnEndDate"
                                    ImageAlign="AbsMiddle" CausesValidation="false" />
                                <ajaxToolkit:CalendarExtender ID="calToDate" runat="server" TargetControlID="txtEndDate"
                                    Format="MM/dd/yyyy" PopupButtonID="imgbtnEndDate" />
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix PMSCategories BulkCategories" id="noOfResources">
                            <div class="LabelDiv">
                                <label>Number Of Resources:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtNoOfResources" CssClass="txtfield" MaxLength="3" runat="server"
                                    onkeypress="return allowOnlyNumber(event);"></asp:TextBox>
                            </div>
                        </div>
                        <div class="rightcol PMSCategories">
                            <div class="LabelDiv">
                                <label>
                                    Resource pool:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblResourcePool" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix PMSCategories">
                            <div class="LabelDiv">
                                <label>Reporting To:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblReportingTo" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    Description:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblDescription" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>Comment And Description:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblCommentDesc" runat="server" Height="100%"></asp:Label>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    Issue Status:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="dropdown">
                                    <%--<asp:ListItem Value="1">New</asp:ListItem>
											<asp:ListItem Value="2">Resolved</asp:ListItem>
											<asp:ListItem Value="4">Reopened</asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>
                                    Cause:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtCause" runat="server" CssClass="" MaxLength="1000" TextMode="MultiLine"
                                    onkeyDown="return checkTextAreaMaxLength(this,event,'1000');" Height="65px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    Fix:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtFix" runat="server" CssClass="" MaxLength="1000" TextMode="MultiLine"
                                    onkeyDown="return checkTextAreaMaxLength(this,event,'1000');" Height="65px"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="formrow clearfix">
                        <div class="leftcol clearfix clsAllocationLink">
                            <div class="LabelDiv">
                                <label>
                                    Resource Allocation:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:LinkButton ID="lknNewAllocation" runat="server" Text="New Resource"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="leftcol clearfix clsUpdateAllocationLink">
                            <div class="LabelDiv">
                                <label>
                                    Resource Allocation:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:LinkButton ID="lnkUpdateCurrentAllocation" runat="server" Text="Update Current Allocation"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="leftcol clearfix clsReAllocationLink">
                            <div class="LabelDiv">
                                <label>
                                    Resource Allocation:
                                </label>
                            </div>
                            <div class="InputDiv">
                                <asp:LinkButton ID="lnkReallocation" runat="server" Text="Reallocation"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="rightcol" style="display: none">
                            <div class="LabelDiv">
                                <asp:Label ID="lblComment" runat="server">Previous Comment:</asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblPreviousComment" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <asp:Button ID="btnAddComment" runat="server" CssClass="ButtonGray" Text="Add Comment" OnClick="btnAddComment_Click"></asp:Button>

                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <asp:Label ID="lblComment1" runat="server" Visible="False">Add Comment:</asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtAddcomment" runat="server" MaxLength="1000" TextMode="MultiLine"
                                    Height="65px" onkeyDown="return checkTextAreaMaxLength(this,event,'1000');"
                                    Visible="False"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <h3 class="SmallHead">Issue History</h3>
                <div class="InnerContainer">
                    <asp:DataGrid ID="dgIssueDetails" runat="server" CssClass="TableJqgrid" Width="100%"
                        EnableViewState="True" CellPadding="5" OnPageIndexChanged="dgIssueDetails_PageChange"
                        AllowPaging="True" PageSize="5" AutoGenerateColumns="False" OnItemDataBound="dgIssueDetails_ItemDataBound">
                        <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                        <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="Assigned To">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "EmployeeName")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Cause">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "Cause")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Fix">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "Fix")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatusID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "StatusID")%>'>
                                                </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Status")%>'> ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Date">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "Date")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>

                    <asp:Label ID="lblCheckHistory" runat="server" CssClass="Error">There are no previous records for this issue.</asp:Label>
                    <div class="clearfix mrgnT15">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="ButtonGray" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                        <asp:Button ID="btnCancel" runat="server" CssClass="ButtonGray" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlMessage" runat="server">
                <asp:Label ID="lblStatusUpdateMsg" runat="server" CssClass="success">Issue status has been successfully updated.</asp:Label>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                </asp:UpdatePanel>
            </asp:Panel>

            <%-- added by nikhil--%>
            <asp:TextBox ID="txtUpdateCurrentAllocation" class="RMGHidden" hidden="true" runat="server"
                Text="<%$appSettings:UpdateCurrentAllocationText %>" />
            <asp:TextBox ID="txtNewResource" class="RMGHidden" hidden="true" runat="server" Text="<%$appSettings:NewResourceText %>" />
            <asp:TextBox ID="txtSingleOrBulkExtension" class="RMGHidden" hidden="true" runat="server"
                Text="<%$appSettings:SingleOrBulkExtensionText %>" />
            <asp:TextBox ID="hdnTxtCategory" class="RMGHidden" hidden="true" runat="server" />
            <asp:TextBox ID="hdnFromDate" class="RMGHidden" hidden="true" runat="server" />
            <asp:TextBox ID="hdnToDate" class="RMGHidden" hidden="true" runat="server" />
            <asp:TextBox ID="hdnReportedByEmpId" class="RMGHidden" hidden="true" runat="server" />
            <asp:TextBox ID="hdnProjectNameId" class="RMGHidden" hidden="true" runat="server" />
            <asp:TextBox ID="hdnStatusOfIssue" hidden="true" runat="server" />
        </div>
    </section>
</asp:Content>