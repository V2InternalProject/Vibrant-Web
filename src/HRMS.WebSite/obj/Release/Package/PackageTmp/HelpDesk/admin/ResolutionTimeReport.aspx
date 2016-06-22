<%@ Page Language="c#" CodeBehind="ResolutionTimeReport.aspx.cs" AutoEventWireup="True"
    Inherits="V2.Helpdesk.web.admin.ResolutionTimeReport" MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="RT" TagName="ReportsTabs" Src="~/HelpDesk/controls/HelpDeskReportsTabs.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <link href="../css/allstyles.css" type="text/css" rel="stylesheet">
    <link href="../themes/aqua.css" rel="stylesheet">
    <script src="../Script/common.js" type="text/javascript"></script>
    <script language="javascript">
        $(document).ready(function () {
            $("#ResolutionTimeTab").removeClass('tabshover').addClass('colored-border');
        });

        function DateRequired() {
            var FromMonth = document.getElementById("MainContent_ddlFromMonth");
            var FromYear = document.getElementById("MainContent_ddlFromYear");
            var ToMonth = document.getElementById("MainContent_ddlToMonth");
            var ToYear = document.getElementById("MainContent_ddlToYear")
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
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div class="MainBody">
            <div class="TabsContainer">
                <RT:ReportsTabs ID="ReportsTabs1" runat="server"></RT:ReportsTabs>

                <section class="clearfix add-detailsdata ResolutionTime">
                    <div class="formrow clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>From Month:</label>
                            </div>
                            <div class="InputDiv MonthYear">
                                <asp:DropDownList ID="ddlFromMonth" runat="server" CssClass="dropdown" Width="150px">
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

                                <asp:DropDownList ID="ddlFromYear" runat="server" CssClass="dropdown" Width="150px"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>To Month:</label>
                            </div>
                            <div class="InputDiv MonthYear">
                                <asp:DropDownList ID="ddlToMonth" runat="server" CssClass="dropdown" Width="150px">
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
                                <asp:DropDownList ID="ddlToYear" runat="server" CssClass="dropdown" Width="150px"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="formrow clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>Member Name:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlEmployeeName" runat="server" CssClass="dropdown"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="rightcol">
                            <div class="LabelDiv">
                                <label>Severity:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="ddlSeverity" runat="server" CssClass="dropdown"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="formrow clearfix">
                        <div class="leftcol">
                            <div class="LabelDiv">
                                <label>Issue Health:</label>
                            </div>
                            <div class="InputDiv ColoredDD">
                                <select class="dropdown" id="ddlIssueHealth" runat="server" style="width: 160px;">
                                    <option value="-1" selected>Select All</option>
                                    <option style="font-weight: lighter; color: black; background-color: #66cc00" value="1">Green</option>
                                    <option style="font-weight: lighter; color: black; background-color: #ffcc33" value="2">Amber</option>
                                    <option style="font-weight: lighter; color: black; background-color: #ff3300" value="3">Red</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="ButtonGray mrgnT10" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                        <!--<asp:Button id="btnGraphicalRepresentation" runat="server" CssClass="btn" Text="View Graphical Representation"></asp:Button>-->
                        <%--  <asp:Button ID="btnSubmit1" runat="server" Text="Submit" OnClick="btnSubmit_Click" />--%>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>

                    <div class="ButtonContainer2 clearfix">
                        <asp:LinkButton ID="lnkbtnGenrateToExcel" OnClick="lnkbtnGenrateToExcel_Click" runat="server" Visible="False" CssClass="ButtonGray">Generate To Excel</asp:LinkButton>
                    </div>

                    <div class="InnerContainer">
                        <asp:DataGrid ID="dgReport" runat="server" CssClass="TableJqgrid" PageSize="10" DataKeyField="ReportIssueID"
                            OnItemCommand="dgReport_ItemCommand" Width="100%" OnItemDataBound="dgReport_ItemDataBound" CellPadding="2"
                            HeaderStyle-CssClass="TableHeader" AutoGenerateColumns="False" OnPageIndexChanged="dgReport_PageChange" AllowPaging="True">
                            <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                            <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="Issue ID">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lBtnIssueID" runat="server" CommandName="viewDetails" Text='<%# DataBinder.Eval(Container.DataItem,"ReportIssueID") %>'>
										</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Issue Reported By">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"ReportedBy") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Issue Reported On">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"ReportedOn") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Problem Severity">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"ProblemSeverity") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Assigned To">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"AssignedTo") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Resolved On">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResolvedOn" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ResolvedOn") %>'>
										</asp:Label>
                                        <%--<%# DataBinder.Eval(Container.DataItem,"ResolvedOn") %>--%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn Visible="False" HeaderText="Current Status">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"StatusID") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Resolution Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResolutionTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ResolutionTime") %>'>
										</asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Issue Health">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssueHealth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ResolutionHealth") %>'>
										</asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>

                    <div class="InnerContainer">
                        <asp:DataGrid ID="Datagrid1" runat="server" AutoGenerateColumns="False" CssClass="TableJqgrid" DataKeyField="ReportIssueID"
                            Width="100%" CellPadding="2" HeaderStyle-CssClass="tableheader" Visible="true">
                            <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                            <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="Issue ID">
                                    <ItemTemplate>

                                        <%# DataBinder.Eval(Container.DataItem,"ReportIssueID") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Issue Reported By">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"ReportedBy") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Issue Reported On">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"ReportedOn") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Problem Severity">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"ProblemSeverity") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Assigned To">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"AssignedTo") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Resolved On">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"ResolvedOn") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn Visible="False" HeaderText="Current Status">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"StatusID") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Resolution Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResolutionTime1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ResolutionTime") %>'>
										</asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Issue Health">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResolutionHealth1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ResolutionHealth") %>'>
										</asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </section>
            </div>
        </div>
    </section>
</asp:Content>