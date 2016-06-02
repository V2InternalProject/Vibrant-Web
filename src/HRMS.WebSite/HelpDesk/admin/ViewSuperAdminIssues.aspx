<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="ViewSuperAdminIssues.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.ViewSuperAdminIssues" MasterPageFile="~/Views/Shared/HRMS.Master" %>

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
            <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
            <div class="InnerContainer TwoFilters clearfix">
                <div class="FormContainerBox leftcol">
                    <div class="formrow clearfix">
                        <div class="LabelDiv">
                            <label>Select Status:</label>
                        </div>
                        <div class="InputDiv">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="dropdown"
                                Width="206px">
                                <%--<asp:ListItem Value="1,4" Selected="True">All Open</asp:ListItem>
										<asp:ListItem Value="1">New</asp:ListItem>
										<asp:ListItem Value="2">Resolved</asp:ListItem>
										<asp:ListItem Value="4">Reopened</asp:ListItem>
										<asp:ListItem Value="3">Moved</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="LabelDiv">
                            <label>Select Employee:</label>
                        </div>
                        <div class="InputDiv">
                            <asp:DropDownList ID="ddlAssignedEmployeees" runat="server" CssClass="dropdown" Width="206px">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="formrow clearfix">
                        <div class="LabelDiv">
                            <label>Select Department:</label>
                        </div>
                        <div class="InputDiv">
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="dropdown" Width="206px">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="OrDiv">OR</div>
                <div class="FormContainerBox rightcol">
                    <div class="formrow clearfix">
                        <div class="LabelDiv">
                            <asp:Label ID="lblIssueIDSearch" runat="server" Text="Issue ID:"></asp:Label>
                        </div>
                        <div class="InputDiv">
                            <asp:TextBox ID="txtIssueIDSearch" runat="server" Width="206px"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtIssueIDSearch"
                                ErrorMessage="Please Enter only Numeric Value" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="ButtonContainer4 clearfix">
                <div class="leftcol">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="mrgnL25 ButtonGray" Text="GO" Width="59px" OnClick="btnSubmit_Click"></asp:Button>
                </div>
                <div class="rightcol">
                    <asp:Button ID="btnSearch" runat="server" Text="Search Issue ID" CssClass="mrgnL25 mrgnR11 ButtonGray" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Clear Filters" CssClass="ButtonGray" OnClick="btnReset_Click" />
                </div>
            </div>
            <div class="InnerContainer mrgnT15">
                <asp:DataGrid AllowPaging="True" AlternatingItemStyle-HorizontalAlign="Left" AlternatingItemStyle-VerticalAlign="Top"
                    AutoGenerateColumns="False" CellPadding="0" DataKeyField="ReportIssueID"
                    ID="dgViewIssues" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top"
                    OnItemDataBound="dgViewIssues_ItemDataBound" runat="server" CssClass="TableJqgrid" Width="100%" OnItemCommand="dgViewIssues_ItemCommand"
                    OnPageIndexChanged="dgViewIssues_PageIndexChanged">

                    <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                    <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                    <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="Issue ID">
                            <HeaderStyle CssClass="bodytxt" Width="7%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblReportIssueID" runat="server" Visible="False"></asp:Label>
                                <asp:LinkButton ID="lnkReportIssueID" CssClass="gridlink" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ReportIssueID")%>'
                                    CommandName="ShowIssueDetails">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="False" HeaderText="IssueAssignmentID">
                            <HeaderStyle CssClass="bodytxt"></HeaderStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblIssueAssignmentID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ReportIssueID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Category" ReadOnly="True" HeaderText="Department">
                            <HeaderStyle HorizontalAlign="Center" Width="9%"></HeaderStyle>
                            <ItemStyle Width="9%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="SubCategory" ReadOnly="True" HeaderText="Category">
                            <HeaderStyle HorizontalAlign="Center" Width="9%"></HeaderStyle>
                            <ItemStyle Width="9%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ProblemSeverity" ReadOnly="True" HeaderText="Severity">
                            <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                            <ItemStyle Width="8%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" ReadOnly="True" HeaderText="Priority">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Name" ReadOnly="True" HeaderText="Reported By ">
                            <HeaderStyle HorizontalAlign="Center" Width="9%"></HeaderStyle>
                            <ItemStyle Width="9%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ReportIssueDate" ReadOnly="True" HeaderText="Reported On">
                            <HeaderStyle HorizontalAlign="Center" Width="9%"></HeaderStyle>
                            <ItemStyle Width="9%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Description" ReadOnly="True" HeaderText="Description">
                            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                            <ItemStyle Width="20%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Status" HeaderText="Status">
                            <HeaderStyle HorizontalAlign="Center" Width="9%"></HeaderStyle>
                            <ItemStyle Width="9%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="AssignTo" HeaderText="Assigned To">
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle Width="10%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ResolutionForamber" ReadOnly="True" HeaderText="SLA (HH)">
                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                            <ItemStyle Width="5%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Remaining Time To Go To Amber Or Red (HH:MM)">
                            <HeaderStyle CssClass="bodytxt" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
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

<%--<uc1:header ID="Header1" runat="server"></uc1:header>--%>