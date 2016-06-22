<%@ Page Language="c#" CodeBehind="IssueStatusReport.aspx.cs" AutoEventWireup="True"
    Inherits="V2.Helpdesk.web.admin.IssueStatusReport" MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>
<%@ Register TagPrefix="RT" TagName="ReportsTabs" Src="~/HelpDesk/controls/HelpDeskReportsTabs.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
    <script lang="javascript">
        $(document).ready(function () {
            $("#SeverityWiseTab").removeClass('tabshover').addClass('colored-border');
            $("#searchReport").addClass('selected');
            $("#MainContent_AdminHeader1_hdnTabClick").val() = 'SearchReport'
        });

        function DateRequired() {
            var Department = document.getElementById("MainContent_ddlDepartment");
            var selectedDepartment = Department.options[Department.selectedIndex].value;
            if (selectedDepartment == 0) {
                alert("Please select Department ");
                return false;
            }

            var FromMonth = document.getElementById("ddlFrommonth");
            var FromYear = document.getElementById("ddlFromyear");
            var ToMonth = document.getElementById("ddlTomonth");
            var ToYear = document.getElementById("ddlToyear")
            var selectedFromMonth = FromMonth.options[FromMonth.selectedIndex].value;
            var selectedToMonth = ToMonth.options[ToMonth.selectedIndex].value;
            var selectedFromYear = FromYear.options[FromYear.selectedIndex].value;
            var selectedToYear = ToYear.options[ToYear.selectedIndex].value;
            /*var selectedFromDate = "1/" + selectedFromMonth + "/"+ selectedFromYear;
            if (selectedToMonth == "01" || selectedToMonth == "03" || selectedToMonth == "05" || selectedToMonth == "07" || selectedToMonth == "08" || selectedToMonth == "10" || selectedToMonth == "12")
            {
            var selectedToDate = "31/" + selectedToMonth + "/"+ selectedToYear;
            }
            else if(selectedToMonth == "04" || selectedToMonth == "06" || selectedToMonth == "09" || selectedToMonth == "11")
            {
            var selectedToDate = "30/" + selectedToMonth + "/"+ selectedToYear;
            }
            else if(selectedToMonth == "02")
            {
            if(selectedToYear%4 == 0)
            {
            var selectedToDate = "29/" + selectedToMonth + "/"+ selectedToYear;
            }
            else if(selectedToYear%4 != 0)
            {
            var selectedToDate = "28/" + selectedToMonth + "/"+ selectedToYear;
            }
            }
            alert(selectedFromDate);
            alert(selectedToDate);*/
            if (selectedFromYear > selectedToYear) {
                alert("Please select a From Year that is less than the To Year.");
                return false;
            }
            else if (selectedFromYear == selectedToYear) {
                if (selectedFromMonth > selectedToMonth) {
                    alert("Please select a From Month that is less than the To Month.");
                    return false;
                }
                if (selectedFromMonth < selectedToMonth) {
                    return true;
                }
            }
            else return true;

            /*if(isRequire("txtFromDate^txtToDate", "From Date^To Date", this.enable))
            {
            return CompDate("txtFromDate","txtToDate","From Date should be lesser than the To Date");
            }
            else
            {
            return false;
            }*/
        }
    </script>
    <%-- new files--%>
    <link href="../../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/Global.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/New%20Design/common.js" type="text/javascript"></script>
    <section class="HelpdeskContainer Container">
        <uc1:AdminHeader ID="AdminHeader2" runat="server"></uc1:AdminHeader>
        <div class="MainBody">
            <div class="TabsContainer">
                <RT:ReportsTabs ID="ReportsTabs1" runat="server"></RT:ReportsTabs>
                <section class="clearfix add-detailsdata IssueStatusReport">
                    <div class="formrow clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>Department:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="dropdown" Width="183px"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="formrow clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>From Month:</label>
                            </div>
                            <div class="InputDiv MonthYear">
                                <asp:DropDownList ID="ddlFrommonth" runat="server" CssClass="dropdown" Width="104px">
                                    <asp:ListItem Value="01" Selected="True">January</asp:ListItem>
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

                                <asp:DropDownList ID="ddlFromyear" runat="server" CssClass="dropdown">
                                    <asp:ListItem Value="01">2006</asp:ListItem>
                                    <asp:ListItem Value="02">2007</asp:ListItem>
                                    <asp:ListItem Value="03">2008</asp:ListItem>
                                    <asp:ListItem Value="04">2009</asp:ListItem>
                                    <asp:ListItem Value="05">2010</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>To Month:</label>
                            </div>
                            <div class="InputDiv MonthYear">
                                <asp:DropDownList ID="ddlTomonth" runat="server" CssClass="dropdown" Width="103px">
                                    <asp:ListItem Value="01" Selected="True">January</asp:ListItem>
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

                                <asp:DropDownList ID="ddlToyear" runat="server" CssClass="dropdown">
                                    <asp:ListItem Value="01">2006</asp:ListItem>
                                    <asp:ListItem Value="02">2007</asp:ListItem>
                                    <asp:ListItem Value="03">2008</asp:ListItem>
                                    <asp:ListItem Value="04">2009</asp:ListItem>
                                    <asp:ListItem Value="05">2010</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="clearfix">
                        <asp:Button ID="btnshow" runat="server" Text="Submit" OnClick="btnshow_Click" CssClass="ButtonGray mrgnT10"></asp:Button>
                    </div>

                    <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>

                    <asp:DataGrid ID="dgIssuestatus" runat="server" CssClass="TableJqgrid mrgnT20" CellPadding="3" CellSpacing="0"
                        PageSize="30" Width="100%">
                        <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                        <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                    </asp:DataGrid>
                </section>
            </div>
        </div>
    </section>
</asp:Content>