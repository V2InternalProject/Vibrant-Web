<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="IssueHealth.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.lblIssueHealth" MasterPageFile="~/Views/Shared/HRMS.Master" %>

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
        <div class="MainBody">
            <asp:Label ID="lblmsg" runat="server" CssClass="error"></asp:Label>

            <div class="InnerContainer mrgnT28">
                <asp:DataGrid ID="dgViewIssuesHealth" CssClass="TableJqgrid" Width="100%" AutoGenerateColumns="False"
                    ItemStyle-VerticalAlign="Top" AlternatingItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left" AlternatingItemStyle-HorizontalAlign="Left"
                    OnItemDataBound="dgViewIssues_ItemDataBound" runat="server">
                    <AlternatingItemStyle HorizontalAlign="Left" VerticalAlign="Top"></AlternatingItemStyle>
                    <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                    <HeaderStyle Wrap="False" CssClass="tableHeaders"></HeaderStyle>
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                    <Columns>
                        <asp:BoundColumn DataField="Category" ItemStyle-Width="30%" ReadOnly="True" HeaderText="Department">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ProblemSeverity" ItemStyle-Width="30%" ReadOnly="True" HeaderText="Severity">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblCategoryID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "categoryId")%>'>
										</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblSeverityID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ProblemSeverityId")%>'>
										</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Issue Health(%)">
                            <ItemTemplate>
                                <asp:Label ID="lblIssueHealth" runat="server" Width="100%" Height="100%" Text='<%#DataBinder.Eval(Container.DataItem, "PercentageIssueHealth")%>'>
										</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </div>
        </div>
    </section>
</asp:Content>