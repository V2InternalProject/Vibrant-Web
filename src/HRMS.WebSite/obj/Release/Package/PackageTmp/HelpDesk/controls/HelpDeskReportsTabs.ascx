<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpDeskReportsTabs.ascx.cs"
    Inherits="HRMS.HelpDesk.controls.HelpDeskReportsTabs" %>
<div class="tabs">
    <ul class="helpdesk-tabs">
        <li id="IssueMembershipTab" class="tabshover">
            <asp:LinkButton ID="IssueMembership" Text="Issue Memberwise" runat="server" OnClick="IssueMembership_Click" CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="IssueDepartmentwiseTab" class="tabshover">
            <asp:LinkButton ID="IssueDepartmentwise" Text="Issue Departmentwise" runat="server" OnClick="IssueDepartmentwise_Click" CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="ResolutionTimeTab" class="tabshover">
            <asp:LinkButton ID="ResolutionTime" Text="Resolution Time" runat="server" OnClick="ResolutionTime_Click" CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="SeverityWiseTab" class="tabshover">
            <asp:LinkButton ID="SeverityWise" Text="Severitywise" runat="server" OnClick="SeverityWise_Click" CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="SummaryReportTab" class="tabshover">
            <asp:LinkButton ID="SummaryReport" Text="Summary Report Department" runat="server"
                OnClick="SummaryReport_Click" CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="SLAReportTab" class="tabshover">
            <asp:LinkButton ID="SLAReport" Text="SLA Report" runat="server" OnClick="SLAReport_Click" CssClass="TabsLinks"></asp:LinkButton></li>
    </ul>
</div>