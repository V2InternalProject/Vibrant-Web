<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrbitMastersTabs.ascx.cs" Inherits="HRMS.Orbitweb.OrbitMastersTabs" %>
<div class="tabs mrgnT25">
    <ul class="helpdesk-tabs">
        <li id="HolidayListTab" class="tabshover">
            <asp:LinkButton ID="HolidayList" Text="Holiday Lists" runat="server" OnClick="HolidayList_Click" CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="ConfigSettingsTab" class="tabshover">
            <asp:LinkButton ID="ConfigSettings" Text="Configure Settings" runat="server" OnClick="ConfigSettings_Click" CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="StatusMasterTab" class="tabshover">
            <asp:LinkButton ID="StatusMaster" Text="Status Master" runat="server" OnClick="StatusMaster_Click" CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="MonthlyLeaveUploadTab" class="tabshover">
            <asp:LinkButton ID="MonthlyLeaveUpload" Text="Monthly Leave Upload" runat="server" OnClick="MonthlyLeaveUpload_Click" CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="ShiftDetailsTab" class="tabshover">
            <asp:LinkButton ID="ShiftDetails" Text="Shift Details" runat="server" OnClick="ShiftDetails_Click" CssClass="TabsLinks"></asp:LinkButton></li>
        <%--        <li id="SummaryReportTab" class="tabshover">
            <asp:LinkButton ID="SummaryReport" Text="Summary Report Department" runat="server"
                OnClick="SummaryReport_Click" CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="SLAReportTab" class="tabshover">
            <asp:LinkButton ID="SLAReport" Text="SLA Report" runat="server" OnClick="SLAReport_Click" CssClass="TabsLinks"></asp:LinkButton></li>--%>
    </ul>
</div>