<%@ Page Language="c#" CodeBehind="SLAReport.aspx.cs" AutoEventWireup="false" Inherits="V2.Helpdesk.web.admin.SLAReport"
    MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>
<%@ Register TagPrefix="RT" TagName="ReportsTabs" Src="~/HelpDesk/controls/HelpDeskReportsTabs.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
    <!-- Loading Calendar JavaScript files -->
    <script src="../utils/zapatec.js" type="text/javascript"></script>
    <script src="../src/calendar.js" type="text/javascript"></script>
    <script language="javascript">
        $(document).ready(function () {
            $("#SLAReportTab").removeClass('tabshover').addClass('colored-border');
        });
        function validateCategory() {
            var ddlCategory = document.getElementById("ddlCategory").value;
            if (ddlCategory == 0) {
                alert("Please select Department ");
                return false;
            }
        }
        function displayPeriod(dropdown) {

            var myIndex = dropdown.selectedIndex
            var selectedPeriod = dropdown.options[myIndex].value;
            //alert(myIndex)
            if (selectedPeriod == "day") {
                document.getElementById("tblParticularDay").style.display = "";
                document.getElementById("tblParticularMonth").style.display = "none";
                document.getElementById("tblRange").style.display = "none";
            }
            if (selectedPeriod == "month") {
                document.getElementById("tblParticularDay").style.display = "none";
                document.getElementById("tblParticularMonth").style.display = "";
                document.getElementById("lblMonth").style.display = "";
                document.getElementById("ddlMonths").style.display = "";
                document.getElementById("tblRange").style.display = "none";
            }
            if (selectedPeriod == "year") {
                document.getElementById("tblParticularDay").style.display = "none";
                document.getElementById("tblParticularMonth").style.display = "";
                document.getElementById("ddlMonths").style.display = "none";
                document.getElementById("lblMonth").style.display = "none";
                document.getElementById("ddlYears").style.display = "";
                document.getElementById("tblRange").style.display = "none";
            }
            if (selectedPeriod == "week") {
                document.getElementById("tblParticularDay").style.display = "";
                document.getElementById("tblParticularMonth").style.display = "none";
                document.getElementById("tblRange").style.display = "none";
            }
            if (selectedPeriod == "range") {
                document.getElementById("tblParticularDay").style.display = "none";
                document.getElementById("tblParticularMonth").style.display = "none";
                document.getElementById("tblRange").style.display = "";
            }
        }
        function onBodyLoad() {
            var dropdown = document.getElementById("ddlPeriod")
            var myIndex = dropdown.selectedIndex
            var selectedPeriod = dropdown.options[myIndex].value;
            //alert(myIndex)
            if (selectedPeriod == "day") {
                document.getElementById("tblParticularDay").style.display = "";
                document.getElementById("tblParticularMonth").style.display = "none";
                document.getElementById("tblRange").style.display = "none";
            }
            if (selectedPeriod == "month") {
                document.getElementById("tblParticularDay").style.display = "none";
                document.getElementById("tblParticularMonth").style.display = "";
                document.getElementById("lblMonth").style.display = "";
                document.getElementById("ddlMonths").style.display = "";
                document.getElementById("tblRange").style.display = "none";
            }
            if (selectedPeriod == "year") {
                document.getElementById("tblParticularDay").style.display = "none";
                document.getElementById("ddlMonths").style.display = "none";
                document.getElementById("lblMonth").style.display = "none";
                document.getElementById("ddlYears").style.display = "";
                document.getElementById("tblRange").style.display = "none";
            }
            if (selectedPeriod == "week") {
                document.getElementById("tblParticularDay").style.display = "";
                document.getElementById("tblParticularMonth").style.display = "none";
                document.getElementById("tblRange").style.display = "none";
            }
            if (selectedPeriod == "range") {
                document.getElementById("tblParticularDay").style.display = "none";
                document.getElementById("tblParticularMonth").style.display = "none";
                document.getElementById("tblRange").style.display = "";
            }
        }

        function DateRequired() {

            //alert("Hi");
            var dropdown = document.getElementById("ddlPeriod")
            var myIndex = dropdown.selectedIndex
            var selectedPeriod = dropdown.options[myIndex].value;
            //alert(myIndex)

            if (selectedPeriod == "day" || selectedPeriod == "week") {
                if (!isRequire("txtDay", "Date", this.enabled)) {
                    return false;
                }
            }
            if (selectedPeriod == "range") {
                //alert("hello");
                if (!isRequire("txtFromDate^txtToDate", "From Date^To Date", this.enabled)) {
                    return false;
                }
                return CompDate("txtFromDate", "txtToDate", "From Date should be lesser than the To Date");
            }
        }

        function CheckDate() {

            //alert("checkDate")
            if (DateRequired()) {
                //alert("dateRrqd clrd");
                if (CheckDateFormat()) {
                    //alert("format checked");
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        function DateRequired() {
            alert(selectedPeriod);
            var dropdown = document.getElementById("ddlPeriod")
            var myIndex = dropdown.selectedIndex
            var selectedPeriod = dropdown.options[myIndex].value;
            //alert(myIndex)
            if (selectedPeriod == "day" || selectedPeriod == "week") {
                if (!isRequire("txtDay", "Date", this.enabled)) {
                    return false;
                }
                else {
                    return true;
                }
            }
            if (selectedPeriod == "range") {
                //alert("hello");
                if (!isRequire("txtFromDate^txtToDate", "From Date^To Date", this.enabled)) {
                    return false;
                }
                else {
                    return CompDate("txtFromDate", "txtToDate", "From Date should be lesser than the To Date");
                }
            }
            if (selectedPeriod == "month" || selectedPeriod == "year") {
                return true;
            }
        }

        function CheckDateFormat() {
            var txtSelectDate = document.getElementById("txtFromDate")
            alert(txtSelectDate);
            var periodDropDown = document.getElementById("ddlPeriod");
            if (yearfield < '1990' || yearfield > '2200') {

                alert("Please select date range between 01/01/1990 - 31/12/2200 range.")
                return false;
            }
            if (periodDropDown.options[periodDropDown.selectedIndex].value == "month" || periodDropDown.options[periodDropDown.selectedIndex].value == "year") {
                //alert("yr/mnth");
                return true;
            }
            else if (periodDropDown.options[periodDropDown.selectedIndex].value == "day" || periodDropDown.options[periodDropDown.selectedIndex].value == "week") {
                //alert("day")
                var validformat = /^\d{2}\/\d{2}\/\d{4}$/       //Basic check for format
                var txtSelectDate = document.getElementById("txtDay")
                if (!validformat.test(txtSelectDate.value)) {
                    alert("Invalid date detected. Please use 'MM/DD/YYYY' format.")
                    return false;
                }
                var monthfield = txtSelectDate.value.split("/")[0]
                var dayfield = txtSelectDate.value.split("/")[1]
                var yearfield = txtSelectDate.value.split("/")[2]

                var dayobj = new Date(yearfield, monthfield - 1, dayfield)
                if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield)) {
                    alert("Invalid date detected. Please use 'MM/DD/YYYY' format.")
                    return false;
                }
                else if (yearfield < '1990' || yearfield > '2200') {

                    alert("Please select date range between 01/01/1990 - 31/12/2200 range.")
                    return false;
                }

                else {
                    return true;
                }
            }
            else if (periodDropDown.options[periodDropDown.selectedIndex].value == "range") {
                var validformat = /^\d{2}\/\d{2}\/\d{4}$/        //Basic check for format
                var FromDate = document.getElementById("txtFromDate")
                if (!validformat.test(FromDate.value)) {
                    alert("Invalid date detected. Please use 'MM/DD/YYYY' format.")
                    return false;
                }
                var monthfield = FromDate.value.split("/")[0]
                var dayfield = FromDate.value.split("/")[1]
                var yearfield = FromDate.value.split("/")[2]
                var dayobj = new Date(yearfield, monthfield - 1, dayfield)
                if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield)) {
                    alert("Invalid date detected. Please use 'MM/DD/YYYY' format.")
                    return false;
                }

                var validformat = /^\d{2}\/\d{2}\/\d{4}$/         //Basic check for format
                var ToDate = document.getElementById("txtToDate")
                if (!validformat.test(ToDate.value)) {
                    alert("Invalid Date Format. Please correct and submit again.")
                    return false;
                }
                var monthfield = ToDate.value.split("/")[0]
                var dayfield = ToDate.value.split("/")[1]
                var yearfield = ToDate.value.split("/")[2]
                var dayobj = new Date(yearfield, monthfield - 1, dayfield)
                if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield)) {
                    alert("Invalid date detected. Please use 'MM/DD/YYYY' format.")
                    return false
                }
                else if (yearfield < '1990' || yearfield > '2200') {

                    alert("Please select date range between 01/01/1990 - 31/12/2200 range.")
                    return false;
                }
                else {
                    return true;
                }
            }
        }
    </script>
    <%-- new files--%>
    <link href="../../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/Global.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/New%20Design/common.js" type="text/javascript"></script>
    <section class="HelpdeskContainer Container">
        <ajaxToolkit:ToolkitScriptManager EnablePartialRendering="true" runat="Server" ID="ScriptManager1" />
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div class="MainBody">
            <div class="TabsContainer">
                <RT:ReportsTabs ID="ReportsTabs1" runat="server"></RT:ReportsTabs>
                <section class="clearfix SLAReport add-detailsdata">
                    <asp:Label ID="lblMsg" CssClass="error" runat="server"></asp:Label>
                    <div class="formrow clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>
                                    <asp:RequiredFieldValidator ValidationGroup="submit" InitialValue="0" ID="rfvDept"
                                        runat="server" ControlToValidate="ddlDepartment" ErrorMessage="Select Department">*</asp:RequiredFieldValidator>Select
                                    Department:</label>
                            </div>
                            <div class="InputDiv MonthYear">
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>
                                    <asp:RequiredFieldValidator ValidationGroup="submit" ID="rfvfromDate" ErrorMessage="Select From Date "
                                        ControlToValidate="txtFromDate" runat="server">*</asp:RequiredFieldValidator>From
                                    Date:</label>
                            </div>
                            <div class="InputDiv MonthYear">
                                <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Width="203px"></asp:TextBox>&nbsp;

                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server"
                                    ID="ibtnFromDateCalendar" ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger mrgnR12" />
                                <ajaxToolkit:CalendarExtender ID="FromDateDatePicker" runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="ibtnFromDateCalendar" />
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    <asp:RequiredFieldValidator ValidationGroup="submit" ID="rfvTodate" ErrorMessage="Select To Date "
                                        ControlToValidate="txtToDate" runat="server">*</asp:RequiredFieldValidator>To
                                    Date:</label>
                            </div>
                            <div class="InputDiv MonthYear">
                                <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Width="203px"></asp:TextBox>&nbsp;

                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server"
                                    ID="ibtnFromToCalendar" ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger mrgnR12" />
                                <ajaxToolkit:CalendarExtender ID="ToDateDatePicker" runat="server" TargetControlID="txtToDate"
                                    PopupButtonID="ibtnFromToCalendar" />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                        <asp:Button ID="btnSubmit" ValidationGroup="submit" runat="server" CssClass="ButtonGray mrgnT10"
                            Text="Submit"></asp:Button>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
                    <asp:CompareValidator ID="CmpDate" runat="server" Visible="True" ControlToValidate="txtToDate"
                        ErrorMessage="The From Date should not be greater than the To Date" ControlToCompare="txtFromDate"
                        Operator="GreaterThanEqual" Type="Date" Display="None"></asp:CompareValidator>
                    <asp:RangeValidator ID="RangetxtFrom" ControlToValidate="txtFromDate" Type="Date"
                        MinimumValue="1/1/1990" MaximumValue="12/31/2200" ErrorMessage="Please select Fromdate between 01/01/1990 - 31/12/2200 range."
                        Display="None" runat="server">
                    </asp:RangeValidator>
                    <asp:RangeValidator ID="RangetxtTo" ControlToValidate="txtToDate" Type="Date" MinimumValue="1/1/1990"
                        MaximumValue="12/31/2200" ErrorMessage="Please select Todate between 01/01/1990 - 31/12/2200 range."
                        Display="None" runat="server">
                    </asp:RangeValidator>
                    <div class="ButtonContainer2 clearfix">
                        <asp:LinkButton ID="lnkbtnPrint" runat="server" CssClass="ButtonGray">Print</asp:LinkButton>
                        <asp:Label ID="lblSeperator" runat="server"></asp:Label>
                        <asp:LinkButton ID="lnkbtnSendToExcel" runat="server" CssClass="ButtonGray">Send To Excel</asp:LinkButton>
                    </div>
                    <div class="scrollHContainer">
                        <asp:GridView ID="dgSLAReport" CssClass="TableJqgrid" runat="server" AutoGenerateColumns="True"
                            CellPadding="5" AllowPaging="True" Width="100%" PageSize="10" RowStyle-VerticalAlign="Top"
                            RowStyle-HorizontalAlign="Center" AlternatingRowStyle-VerticalAlign="Top" OnPageIndexChanging="dgSLAReport_PageIndexChanging1">
                            <HeaderStyle CssClass="tableHeaders" />
                            <PagerSettings FirstPageImageUrl="~/Images/New Design/prev.png" FirstPageText=""
                                LastPageImageUrl="~/Images/New Design/next.png" LastPageText="" NextPageText="Next"
                                PreviousPageText="Prev" />
                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" />
                            <RowStyle CssClass="tableRows" />
                        </asp:GridView>
                    </div>
                    <asp:ValidationSummary ID="SLAValidationSummary1" ValidationGroup="submit" runat="server"
                        ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary>
                </section>
            </div>
        </div>
    </section>
</asp:Content>