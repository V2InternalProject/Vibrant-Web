<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="true"
    CodeBehind="StartWorkflows.aspx.cs" Inherits="HRMS.Orbitweb.StartWorkflows" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(document).ready(function () {

            $("#btnStop").attr("disabled", "disabled");
            $("#btnOnDemandLeaveBalance").attr("disabled", "disabled");
            $("#btnACStop").attr("disabled", "disabled");
            $("#btnOnDemandAttendencCheck").attr("disabled", "disabled");
            $("#btnLUStop").attr("disabled", "disabled");
            $("#btnLURefreshLeaveCredit").attr("disabled", "disabled");

            $("#btnStop").attr("readonly", true);
            $("#btnOnDemandLeaveBalance").attr("readonly", true);
            $("#btnACStop").attr("readonly", true);
            $("#btnOnDemandAttendencCheck").attr("disabled", "disabled");
            $("#btnLUStop").attr("readonly", true);
            $("#btnLURefreshLeaveCredit").attr("readonly", true);

        });
    </script>
    <section class="AttendanceContainer Container">
        <div class="FixedHeader AdminHeader">
            <div class="clearfix">
                <h2 class="MainHeading">Administration</h2>
            </div>
            <nav class="sub-menu-colored">
                <a href="AdminApproval.aspx">Admin Approval</a> <a href="LeaveTransaction.aspx">Leave
                    Transaction</a> <%--<a href="LeaveAnomalyReport.aspx">Leave Anomaly Report</a>--%> <a href="BulkEntries.aspx">Bulk Entries</a> <a href="AdminLeaveApplicationForm.aspx">Admin Leave Application</a><%--<a
                            href="StartWorkflows.aspx" class="selected">Manage Processes</a>--%> <a href="HolidayList.aspx">Masters</a>
            </nav>
        </div>
        <div class="MainBody">
            <div class="OrbitAuto">
                <div>
                    <asp:Label runat="Server" ID="lblSuccess" SkinID="lblSuccess"></asp:Label>
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </div>
                <div class="mainContentPad clearfix">
                    <asp:Label Text="Update Balance Process Actions" runat="server" ID="lblLeaveBalanceWF"
                        class="actionHeading"></asp:Label>
                    <div class="buttonWrap clearfix">
                        <asp:Button runat="server" ID="btnStart" Text="Start" OnClick="btnStart_Click" class="ButtonGray" />
                        <asp:Button runat="server" ID="btnStop" Text="Stop" class="ButtonGray" OnClick="btnStop_Click" />
                        <asp:Button runat="server" ID="btnOnDemandLeaveBalance" Text="Refresh Leaves" OnClick="btnOnDemandLeaveBalance_Click"
                            class="ButtonGray" />
                    </div>
                    <asp:Label Text="Attendance Check Process Actions" runat="server" ID="lblAttedance"
                        class="actionHeading"></asp:Label>
                    <div class="buttonWrap clearfix">
                        <asp:Button runat="server" ID="btnACStart" Text="Start" OnClick="btnACStart_Click"
                            class="ButtonGray" />
                        <asp:Button runat="server" ID="btnACStop" Text="Stop" OnClick="btnACStop_Click" class="ButtonGray" />
                        <asp:Button runat="server" ID="btnOnDemandAttendencCheck" Text="Refresh Attendence Check"
                            OnClick="btnOnDemandAttendencCheck_Click" class="ButtonGray" />
                    </div>
                    <asp:Label Text="Monthly Leave Credit Process Actions" runat="server" ID="lblLeaveUpload"
                        class="actionHeading"></asp:Label>
                    <div class="buttonWrap clearfix">
                        <asp:Button runat="server" ID="btnLUStart" Text="Start" OnClick="btnLUStart_Click"
                            class="ButtonGray" />
                        <asp:Button runat="server" ID="btnLUStop" Text="Stop" OnClick="btnLUStop_Click" class="ButtonGray" />
                        <asp:Button runat="server" ID="btnLURefreshLeaveCredit" Text="On Demand Leaves Credit"
                            OnClick="btnLURefreshLeaveCredit_Click" class="ButtonGray" />
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>