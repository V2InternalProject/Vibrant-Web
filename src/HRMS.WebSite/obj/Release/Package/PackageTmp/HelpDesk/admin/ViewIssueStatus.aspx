<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="ViewIssueStatus.aspx.cs" AutoEventWireup="True"
    Inherits="V2.Helpdesk.web.admin.ViewIssueStatus" MasterPageFile="~/Views/Shared/HRMS.Master" %>

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
                <a href="javascript:history.back();" class="BackLink">Back to Reports</a>
            </div>
            <asp:Panel ID="pnlIssueDetails" runat="server">
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
                                <label>Issue Reported on:</label>
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
                                <asp:Label ID="lblSubCategory_Category" runat="server" CssClass="trcolor"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>Problem Severity:</label>
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
                                <asp:Label ID="lblDescription" runat="server" CssClass="trcolor"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="leftcol clearfix">
                            <div class="LabelDiv">
                                <label>Report Status:</label>
                            </div>
                            <div class="InputDiv">
                                <asp:Label ID="lblReportStatus" runat="server" CssClass="trcolor"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="InnerContainer">
                    <asp:DataGrid ID="dgIssueDetails" runat="server" CssClass="TableJqgrid mrgnT15" OnItemDataBound="dgIssueDetails_ItemDataBound"
                        AutoGenerateColumns="False" PageSize="5" AllowPaging="True" Width="100%" OnPageIndexChanged="dgIssueDetails_PageChange">
                        <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                        <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="Username">
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
                                <%--<ItemTemplate>
														<asp:Label ID="lblStatusID" Runat="server" text='<%#DataBinder.Eval(Container.DataItem, "Status")%>'>
														</asp:Label>
													</ItemTemplate>--%>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "Status")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Status">
                                <%--<ItemTemplate>
														<asp:Label ID="lblStatus" Runat="server"></asp:Label>
													</ItemTemplate>--%>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "Status")%>
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
                <div id="tblAddComments">
                    <input class="ButtonGray" id="btnAddComments" type="button" value="Add Comments" name="btnAddComments"
                        runat="server" onserverclick="btnAddComments_ServerClick">
                </div>

                <asp:Label ID="lblMessage" runat="server" Visible="True" CssClass="success"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="pnlAddComments" runat="server">
                <div class="prefix">
                    Please write your comments in the box below. This will be appended to your problem
                            and may help you get solved soon.
                </div>
                <asp:TextBox ID="txtComments" runat="server" CssClass="txtfield" Width="720px" TextMode="MultiLine"
                    Height="82px"></asp:TextBox>

                <asp:Button ID="btnReOpenIssue" runat="server" CssClass="ButtonGray" Text="Reopen Issue"
                    OnClick="btnReOpenIssue_Click"></asp:Button>
            </asp:Panel>
        </div>
    </section>
</asp:Content>