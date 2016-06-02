<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpDeskMastersTabs.ascx.cs"
    Inherits="HRMS.HelpDesk.controls.HelpDeskMastersTabs" %>
<div class="tabs">
    <ul class="helpdesk-tabs">
        <li id="UserMasterTab">
            <asp:LinkButton ID="UserMaster" Text="User Master" runat="server" OnClick="UserMaster_Click"
                CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="CategoryMasterTab">
            <asp:LinkButton ID="CategoryMaster" Text="Category Master" runat="server" OnClick="CategoryMaster_Click"
                CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="SeverityMasterTab">
            <asp:LinkButton ID="SeverityMaster" Text="Severity Master" runat="server" OnClick="SeverityMaster_Click"
                CssClass="TabsLinks"></asp:LinkButton></li>
        <li id="ResolutionTimeMasterTab">
            <asp:LinkButton ID="ResolutionTimeMaster" Text="Resolution Time Master" runat="server" OnClick="ResolutionTimeMaster_Click"
                CssClass="TabsLinks"></asp:LinkButton></li>
    </ul>
</div>