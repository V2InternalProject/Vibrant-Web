<%@ Page Language="c#" CodeBehind="CategoryWiseSearchReport.aspx.cs" AutoEventWireup="True"
    Inherits="V2.Helpdesk.web.CategoryWiseSearchReport" MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="RT" TagName="ReportsTabs" Src="~/HelpDesk/controls/HelpDeskReportsTabs.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
    <!-- Loading Calendar JavaScript files -->
    <script src="../utils/zapatec.js" type="text/javascript"></script>
    <script src="../src/calendar.js" type="text/javascript"></script>
    <!-- Loading language definition file -->
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
    <script language="javascript">
        $(document).ready(function () {
            $("#IssueDepartmentwiseTab").removeClass('tabshover').addClass('colored-border');
            onBodyLoad();
        });
        function validateCategory() {
            var ddlCategory = document.getElementById("MainContent_ddlCategory").value;
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
                document.getElementById("MainContent_lblMonth").style.display = "";
                $("#MainContent_ddlMonths").next().show();
                document.getElementById("tblRange").style.display = "none";
            }
            if (selectedPeriod == "year") {
                document.getElementById("tblParticularDay").style.display = "none";
                document.getElementById("tblParticularMonth").style.display = "";
                $("#MainContent_ddlMonths").next().hide();
                document.getElementById("MainContent_lblMonth").style.display = "none";
                $("#MainContent_ddlYears").next().show();
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
            var dropdown = document.getElementById("MainContent_ddlPeriod")
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
                document.getElementById("MainContent_lblMonth").style.display = "";
                $("#MainContent_ddlMonths").next().show();
                document.getElementById("tblRange").style.display = "none";
            }
            if (selectedPeriod == "year") {
                document.getElementById("tblParticularDay").style.display = "none";
                $("#MainContent_ddlMonths").next().hide();
                document.getElementById("MainContent_lblMonth").style.display = "none";
                $("#MainContent_ddlYears").next().show();
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
            var dropdown = document.getElementById("MainContent_ddlPeriod")
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
            //alert("hi");
            var dropdown = document.getElementById("MainContent_ddlPeriod")
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
            //alert("in Format")
            var periodDropDown = document.getElementById("MainContent_ddlPeriod");
            if (periodDropDown.options[periodDropDown.selectedIndex].value == "month" || periodDropDown.options[periodDropDown.selectedIndex].value == "year") {
                //alert("yr/mnth");
                return true;
            }
            else if (periodDropDown.options[periodDropDown.selectedIndex].value == "day" || periodDropDown.options[periodDropDown.selectedIndex].value == "week") {
                //alert("day")
                var validformat = /^\d{2}\/\d{2}\/\d{4}$/       //Basic check for format
                var txtSelectDate = document.getElementById("MainContent_txtDay")
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

                    alert("Please select From date between 01/01/1990 - 31/12/2200 range.")
                    return false;
                }
                else {
                    return true;
                }
            }
            else if (periodDropDown.options[periodDropDown.selectedIndex].value == "range") {
                var validformat = /^\d{2}\/\d{2}\/\d{4}$/        //Basic check for format
                var FromDate = document.getElementById("MainContent_txtFromDate")
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
                else if (yearfield < '1990' || yearfield > '2200') {

                    alert("Please select From date between 01/01/1990 - 31/12/2200 range.")
                    return false;
                }

                var validformat = /^\d{2}\/\d{2}\/\d{4}$/         //Basic check for format
                var ToDate = document.getElementById("MainContent_txtToDate")
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

                    alert("Please select To date between 01/01/1990 - 31/12/2200 range.")
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
                <section class="add-detailsdata clearfix DepartmentWise">
                    <div class="formrow clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>
                                    Department Name:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged1"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    Category wise:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="dropdown">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>
                                    Status:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="dropdown">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    Period:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropdown" onChange="displayPeriod(this)">
                                    <asp:ListItem Value="day">Particular Day</asp:ListItem>
                                    <asp:ListItem Value="week">Particular Week</asp:ListItem>
                                    <asp:ListItem Value="month">Particular Month</asp:ListItem>
                                    <asp:ListItem Value="year">Particular Year</asp:ListItem>
                                    <asp:ListItem Value="range">Range of Dates</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix" id="tblParticularDay">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <asp:Label ID="lblDay" runat="server">Day:</asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtDay" runat="server" CssClass="txt" Width="203px"></asp:TextBox>
                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server"
                                    ID="ibtnDayCalendar" ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger mrgnR12" />
                                <ajaxToolkit:CalendarExtender ID="DayDatePicker" runat="server" TargetControlID="txtDay"
                                    PopupButtonID="ibtnDayCalendar" />
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix" id="tblParticularMonth">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <asp:Label ID="lblMonth" runat="server">Month:</asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlMonths" runat="server" CssClass="dropdown">
                                    <asp:ListItem Value="01">January</asp:ListItem>
                                    <asp:ListItem Value="02">February</asp:ListItem>
                                    <asp:ListItem Value="03">March</asp:ListItem>
                                    <asp:ListItem Value="04">April</asp:ListItem>
                                    <asp:ListItem Value="05">May</asp:ListItem>
                                    <asp:ListItem Value="06">June</asp:ListItem>
                                    <asp:ListItem Value="07">July</asp:ListItem>
                                    <asp:ListItem Value="08">August</asp:ListItem>
                                    <asp:ListItem Value="09">September</asp:ListItem>
                                    <asp:ListItem Value="10">October</asp:ListItem>
                                    <asp:ListItem Value="11">November</asp:ListItem>
                                    <asp:ListItem Value="12">December</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    Year:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlYears" runat="server" CssClass="dropdown">
                                    <asp:ListItem Value="2006">2006</asp:ListItem>
                                    <asp:ListItem Value="2007">2007</asp:ListItem>
                                    <asp:ListItem Value="2008">2008</asp:ListItem>
                                    <asp:ListItem Value="2009">2009</asp:ListItem>
                                    <asp:ListItem Value="2010">2010</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix" id="tblRange">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>
                                    From Date:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="txt" Width="203px"></asp:TextBox>
                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server"
                                    ID="ibtnFromDateCalendar" ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger mrgnR12" />
                                <ajaxToolkit:CalendarExtender ID="FromDateDatePicker" runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="ibtnFromDateCalendar" />
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>
                                    To Date:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="txt" Width="203px"></asp:TextBox>
                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server"
                                    ID="ibtnFromToCalendar" ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger mrgnR12" />
                                <ajaxToolkit:CalendarExtender ID="ToDateDatePicker" runat="server" TargetControlID="txtToDate"
                                    PopupButtonID="ibtnFromToCalendar" />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="ButtonGray" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
                    <div class="ButtonContainer2 clearfix">
                        <asp:LinkButton ID="lnkbtnPrint" runat="server" OnClick="lnkbtnPrint_Click" CssClass="ButtonGray">Print</asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnSendToExcel" runat="server" OnClick="lnkbtnSendToExcel_Click"
                            CssClass="ButtonGray">Send To Excel</asp:LinkButton>
                    </div>
                    <div class="InnerContainer">
                        <asp:DataGrid ID="dgReport" runat="server" CssClass="TableJqgrid" AutoGenerateColumns="False"
                            DataKeyField="ReportIssueID" OnItemCommand="dgReport_ItemCommand" Width="100%"
                            AllowPaging="True" OnPageIndexChanged="dgReport_PageIndexChanged" PageSize="10"
                            OnItemDataBound="dgReport_ItemDataBound" CellPadding="5">
                            <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                            <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="Issue ID">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnIssueID" CommandName="ViewDetails" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ReportIssueID")%>'>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="Name" HeaderText="Issue Reported By"></asp:BoundColumn>
                                <asp:BoundColumn DataField="ReportIssueDate" HeaderText="Issue Reported On"></asp:BoundColumn>
                                <asp:BoundColumn DataField="ProblemSeverity" HeaderText="Problem Severity"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Category" HeaderText="Department"></asp:BoundColumn>
                                <asp:BoundColumn DataField="SubCategory" HeaderText="Category"></asp:BoundColumn>
                                <asp:BoundColumn DataField="EmployeeName" HeaderText="Assigned To"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="StatusID" HeaderText="StatusID"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                        <asp:Label ID="lblSeperator" runat="server"></asp:Label>
                    </div>
                </section>
            </div>
        </div>
    </section>
</asp:Content>