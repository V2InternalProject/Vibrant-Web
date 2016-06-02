<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="ViewEmployeeDetails.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.ViewEmployeeDetails" MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="MT" TagName="MasterHeader" Src="~/HelpDesk/controls/HelpDeskMastersTabs.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
    <%-- new files--%>
    <link href="../../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/Global.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/New%20Design/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#UserMasterTab").removeClass('tabshover').addClass('colored-border');
        });
    </script>
    <section class="HelpdeskContainer Container">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div class="MainBody">
            <div class="TabsContainer">
                <MT:MasterHeader ID="MasterHeader1" runat="server"></MT:MasterHeader>
                <section class="add-detailsdata clearfix UserMaster">
                    <div class="ButtonContainer2 clearfix">
                        <asp:LinkButton ID="lnkAddNew" runat="server" OnClick="lnkAddNew_Click" CssClass="ButtonGray">Assign Categories to New User</asp:LinkButton>
                    </div>

                    <asp:DataGrid ID="dgEmployeedetails" AllowSorting="True" Width="100%" runat="server" DataKeyField="EmployeeId"
                        AutoGenerateColumns="False" AllowPaging="True" CssClass="TableJqgrid" OnItemCommand="dgEmployeedetails_ItemCommand"
                        PageSize="10" OnPageIndexChanged="dgEmployeedetails_PageIndexChanged">
                        <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                        <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                        <Columns>
                            <asp:BoundColumn DataField="EmployeeId" HeaderText="Employee Id">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="EmployeeName" HeaderText="Employee Name">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:ButtonColumn Text="View Details" CommandName="Detail"></asp:ButtonColumn>
                            <asp:ButtonColumn Text="Edit Details" CommandName="Edit"></asp:ButtonColumn>
                        </Columns>
                    </asp:DataGrid>
                </section>
            </div>
        </div>
    </section>
</asp:Content>