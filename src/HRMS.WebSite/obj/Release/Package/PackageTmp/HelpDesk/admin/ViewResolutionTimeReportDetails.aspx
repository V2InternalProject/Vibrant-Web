<%@ Page Language="c#" CodeBehind="ViewResolutionTimeReportDetails.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.ViewResolutionTimeReportDetails" MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
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
            <div class="clearfix InnerContainer">
                <a href="javascript:history.back()" class="BackLink">Back To Reports</a>
            </div>
            <div class="FormContainerBox ViewSelected">
                <div class="formrow clearfix">
                    <div class="leftcol clearfix">
                        <div class="LabelDiv">
                            <label>Issue ID:</label>
                        </div>
                        <div class="InputDiv">
                            <asp:Label ID="lblIssueID" runat="server" CssClass="trcolor"></asp:Label>
                        </div>
                    </div>
                    <div class="rightcol clearfix">
                        <div class="LabelDiv">
                            <label>
                                Issue Reported By:
                            </label>
                        </div>
                        <div class="InputDiv">
                            <asp:Label ID="lblReportedBy" runat="server" CssClass="trcolor"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="formrow clearfix">
                    <div class="leftcol clearfix">
                        <div class="LabelDiv">
                            <label>Issue reported on:</label>
                        </div>
                        <div class="InputDiv">
                            <asp:Label ID="lblReportedOn" runat="server" CssClass="trcolor"></asp:Label>
                        </div>
                    </div>
                    <div class="rightcol clearfix">
                        <div class="LabelDiv">
                            <label>
                                Category:
                            </label>
                        </div>
                        <div class="InputDiv">
                            <asp:Label ID="lblSubCategory" runat="server" CssClass="trcolor"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="formrow clearfix">
                    <div class="leftcol clearfix">
                        <div class="LabelDiv">
                            <label>Problem severity:</label>
                        </div>
                        <div class="InputDiv">
                            <asp:Label ID="lblSeverity" runat="server" CssClass="trcolor"></asp:Label>
                        </div>
                    </div>
                    <div class="rightcol clearfix">
                        <div class="LabelDiv">
                            <label>
                                Problem Description:
                            </label>
                        </div>
                        <div class="InputDiv">
                            <asp:Label ID="lblDescription" runat="server" CssClass="trcolor" Width="800px"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="formrow clearfix">
                    <div class="leftcol clearfix">
                        <div class="LabelDiv">
                            <label>Report status:</label>
                        </div>
                        <div class="InputDiv">
                            <asp:Label ID="lblReportStatus" runat="server" CssClass="trcolor"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="InnerContainer">
                <asp:DataGrid ID="dgIssueDetails" runat="server" CssClass="TableJqgrid mrgnT15" AutoGenerateColumns="False"
                    PageSize="5" AllowPaging="True" Width="100%" OnPageIndexChanged="dgIssueDetails_PageChange"
                    OnItemDataBound="dgIssueDetails_ItemDataBound">
                    <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                    <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="User Name">
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
                        <asp:TemplateColumn HeaderText="Status" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblStatusID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "StatusID")%>'>
										</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Status")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Date">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "Date")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </div>
        </div>
    </section>
</asp:Content>