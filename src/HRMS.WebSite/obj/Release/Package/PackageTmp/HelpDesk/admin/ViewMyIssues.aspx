<%@ Page Language="c#" CodeBehind="ViewMyIssues.aspx.cs" AutoEventWireup="True"
    Inherits="V2.Helpdesk.web.admin.ViewMyIssues" MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
    <%-- new files--%>
    <link href="../../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/Global.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/New%20Design/common.js" type="text/javascript"></script>
    <section class="HelpdeskContainer Container">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div class="MainBody HelpdeskMainbody">
            <asp:Label ID="lblMsg" CssClass="error" runat="server"></asp:Label>
            <div class="FormContainerBox">
                <div class="formrow clearfix">
                    <div class="leftcol">
                        <div class="LabelDiv">
                            <asp:Label ID="Label1" runat="server"> Select Status:</asp:Label>
                        </div>
                        <div class="InputDiv">
                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" CssClass="dropdown" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                <%--<asp:ListItem Value="1,4" Selected="True">All Open</asp:ListItem>
										<asp:ListItem Value="1">New</asp:ListItem>
										<asp:ListItem Value="2">Resolved</asp:ListItem>
										<asp:ListItem Value="4">Reopened</asp:ListItem>
										<asp:ListItem Value="3">Moved</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="InnerContainer">
                <asp:DataGrid ID="dgViewIssues" runat="server" DataKeyField="ReportIssueID" AllowPaging="True"
                    CellPadding="2" CssClass="TableJqgrid" OnItemDataBound="dgViewIssues_ItemDataBound"
                    Width="100%" AutoGenerateColumns="False"
                    OnItemCommand="dgViewIssues_ItemCommand" OnPageIndexChanged="dgViewIssues_PageIndexChanged">
                    <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                    <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="IssueID">
                            <HeaderStyle></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblReportIssueID" runat="server" Visible="False"></asp:Label>
                                <asp:LinkButton ID="lnkReportIssueID" runat="server" CommandName="ShowIssueDetails"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.ReportIssueID")%>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ReportIssueID" ReadOnly="True" HeaderText="IssueID">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Category" ReadOnly="True" HeaderText="Department">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="SubCategory" ReadOnly="True" HeaderText="Category">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ProblemSeverity" ReadOnly="True" HeaderText="Severity">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblStatusID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="AssignTo" HeaderText="Reported BY"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ReportIssueDate" ReadOnly="True" HeaderText="Reported On">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Description" ReadOnly="True" HeaderText="Description">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Status" ReadOnly="True" HeaderText="Status">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="SLA" ReadOnly="True" HeaderText="SLA (HH)">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Remaining Time To Go TO Amber Or Red (HH:MM)">
                            <HeaderStyle></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="RemainingTimeToGoTOAmberOrRed" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RemainingTimeToGoTOAmberOrRed") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </div>
        </div>
    </section>
</asp:Content>