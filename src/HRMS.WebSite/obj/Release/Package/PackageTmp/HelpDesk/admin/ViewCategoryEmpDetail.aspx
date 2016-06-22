<%@ Page Language="c#" CodeBehind="ViewCategoryEmpDetail.aspx.cs" AutoEventWireup="True"
    Inherits="V2.Helpdesk.web.admin.ViewCategoryEmpDetail" MasterPageFile="~/Views/Shared/HRMS.Master" %>

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
        <div class="MainBody UserMasterView">
            <div class="clearfix">
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="BackLink" OnClick="btnBack_Click"></asp:Button>
            </div>

            <div class="InnerContainer clearL">
                <h3 class="mainHead">User Master View</h3>
                <div class="clearfix mrgnT20">
                    <div class="floatL">
                        <label class="prefix">Employee Id:</label>
                        <asp:Label ID="lblEmployeeId" runat="server" CssClass="suffix"></asp:Label>
                    </div>
                    <div class="floatL">
                        <label class="prefix">Employee Name:</label>
                        <asp:Label ID="lblEmployeeName" runat="server" CssClass="suffix"></asp:Label>
                    </div>
                </div>

                <h4 class="SmallHeading">User Rights</h4>
            </div>

            <asp:Repeater ID="RepSubCategory" runat="server" Visible="True">

                <ItemTemplate>
                    <div class="FormContainerBox mrgnT20">
                        <div class="mrgnB10">
                            <label class="prefix">Department:</label>
                            <asp:Label ID="lblSystemAdmin" runat="server" CssClass="suffix"></asp:Label>
                        </div>

                        <asp:Label ID="lblSubCatagory" runat="server" CssClass="prefix"></asp:Label>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>
</asp:Content>