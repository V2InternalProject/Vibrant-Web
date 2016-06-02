<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="MemberWiseSearchReport.aspx.cs" AutoEventWireup="True"
    Inherits="V2.Helpdesk.web.admin.MemberWiseSearchReport" MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="RT" TagName="ReportsTabs" Src="~/HelpDesk/controls/HelpDeskReportsTabs.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#IssueMembershipTab").removeClass('tabshover').addClass('colored-border');
            onBodyLoad();
        });
    </script>
    <script language="javascript">
        function displayPeriod(dropdown) {
            var myIndex = dropdown.selectedIndex
            var selectedPeriod = dropdown.options[myIndex].value;
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

        function CheckDate() {
            var dropdown = document.getElementById("MainContent_ddlPeriod")
            var myIndex = dropdown.selectedIndex
            var selectedPeriod = dropdown.options[myIndex].value;
            if (selectedPeriod == "day" || selectedPeriod == "week") {
                var txtDayfield = document.getElementById("MainContent_txtDay").value;
                if (txtDayfield == "") {
                    alert("Please enter Day."); return false;
                }
            }
            if (selectedPeriod == "range") {
                var txtFromDatefield = document.getElementById("MainContent_txtFromDate").value;
                if (txtFromDatefield == "") {
                    alert("Please enter From Date."); return false;
                }
                else {
                    var txtFromDatefield = document.getElementById("MainContent_txtToDate").value;
                    if (txtFromDatefield == "") {
                        alert("Please enter To Date."); return false;
                    }
                }
            }
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
            //alert("in Format")
            var periodDropDown = document.getElementById("ddlPeriod");
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
                else if (yearfield < '1990' || yearfield > '2200') {

                    alert("Please select From date between 01/01/1990 - 31/12/2200 range.")
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
                <section class="clearfix MemberWise add-detailsdata">

                    <div class="formrow clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>Employee Name:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlEmployeeName" CssClass="dropdown" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>Status:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlStatus" CssClass="dropdown" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="formrow clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>Period:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlPeriod" CssClass="dropdown" runat="server" onChange="displayPeriod(this)">
                                    <asp:ListItem Value="day">Particular Day</asp:ListItem>
                                    <asp:ListItem Value="week">Particular Week</asp:ListItem>
                                    <asp:ListItem Value="month">Particular Month</asp:ListItem>
                                    <asp:ListItem Value="year">Particular Year</asp:ListItem>
                                    <asp:ListItem Value="range">Range of Dates</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="rightcol" id="tblParticularDay">
                            <div class="LabelDiv">
                                <asp:Label ID="lblDay" runat="server">Day:</asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtDay" CssClass="txt" runat="server" Width="203px"></asp:TextBox>
                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="ibtnDayCalendar"
                                    ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger mrgnR12" />
                                <ajaxToolkit:CalendarExtender ID="DayDatePicker" runat="server"
                                    TargetControlID="txtDay" PopupButtonID="ibtnDayCalendar" />
                            </div>
                        </div>
                    </div>

                    <div class="formrow clearfix" id="tblParticularMonth">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <asp:Label ID="lblMonth" runat="server">Month:</asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlMonths" CssClass="dropdown" runat="server">
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
                        <div class="rightcol" id="Div1">
                            <div class="LabelDiv">
                                <asp:Label ID="Label1" runat="server">Year:</asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlYears" CssClass="dropdown" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="formrow clearfix" id="tblRange">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <asp:Label ID="Label2" runat="server">From Date:</asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Width="203px"></asp:TextBox>

                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="ibtnFromDateCalendar"
                                    ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger mrgnR12" />
                                <ajaxToolkit:CalendarExtender ID="FromDateDatePicker" runat="server"
                                    TargetControlID="txtFromDate" PopupButtonID="ibtnFromDateCalendar" />
                            </div>
                        </div>
                        <div class="rightcol" id="Div3">
                            <div class="LabelDiv">
                                <asp:Label ID="Label3" runat="server">To Date:</asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Width="203px"></asp:TextBox>

                                <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server" ID="ibtnFromToCalendar"
                                    ImageAlign="AbsMiddle" CausesValidation="False" CssClass="ui-datepicker-trigger mrgnR12" />
                                <ajaxToolkit:CalendarExtender ID="ToDateDatePicker" runat="server"
                                    TargetControlID="txtToDate" PopupButtonID="ibtnFromToCalendar" />
                            </div>
                        </div>
                    </div>

                    <div class="clearfix">
                        <asp:Button ID="btnSubmit" CssClass="ButtonGray mrgnT10" runat="server" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                    </div>
                    <asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label>

                    <div class="ButtonContainer2 clearfix">
                        <asp:LinkButton ID="lBtnPrint" runat="server" OnClick="lBtnPrint_Click" CssClass="ButtonGray">Print</asp:LinkButton>
                        <asp:LinkButton ID="lBtnExcel" runat="server" OnClick="lBtnExcel_Click" CssClass="ButtonGray">Send To Excel</asp:LinkButton>
                    </div>

                    <div class="InnerContainer">
                        <asp:DataGrid ID="dgReport" CssClass="TableJqgrid" runat="server" Width="100%" AllowPaging="True" DataKeyField="ReportIssueID"
                            OnItemCommand="dgReport_ItemCommand" OnPageIndexChanged="dgReport_Pagination"
                            CellPadding="5" AutoGenerateColumns="False" OnItemDataBound="dgReport_ItemDataBound">
                            <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                            <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />

                            <Columns>
                                <asp:TemplateColumn HeaderText="Issue ID">
                                    <ItemTemplate>
                                        <asp:LinkButton CommandName="viewDetails" ID="lBtnIssueID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ReportIssueID")%>'>
										</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Issue Reported By">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container.DataItem,"Name")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Issue Reported On">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container.DataItem,"ReportIssueDate")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Problem Severity">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container.DataItem,"ProblemSeverity")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Category">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container.DataItem,"SubCategoryID")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Assigned To">
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container.DataItem,"EmployeeName")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="StatusID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"StatusID")%>'>
										</asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusName" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </section>
            </div>
        </div>
    </section>
    <asp:Label ID="lblSeparator" runat="server">|</asp:Label>
</asp:Content>