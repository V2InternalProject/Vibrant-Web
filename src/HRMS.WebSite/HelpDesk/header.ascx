<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="header.ascx.cs" Inherits="V2.Helpdesk.web.controls.header" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!--<table width="100%" border="0" cellpadding="0" cellspacing="0" align="center">
	<tr>
		<td align="center">HELPDESK</td>
	</tr>
	<tr>
		<td align="center" height="5"></td>
	</tr>
	<tr>
		<td align="center"><a href=ReportIssue.aspx>Report Issue</a> | <a href=ViewMyStatus.aspx>Issue Status</a></td>
	</tr>
</table> -->

<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <!-- Header starts here-->
    <%--	<tr>
		<td class="header-bg">&nbsp;&nbsp;<span class=page-header>Helpdesk System</span></td>
	</tr>--%>
    <!-- header ends here -->
    <!-- Navs starts here-->
    <tr>
        <td>
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <!--<TD class="greybg" width="5">&nbsp;</TD>-->
                        <td class="navs" valign="middle" align="center" width="150"><a href="ReportIssue.aspx" class="btn1">Report
								Issue</a></td>
                        <td valign="middle" width="2"></td>
                        <td class="navs" valign="middle" align="center" width="150"><a href="ViewMyStatus.aspx" class="btn1">Issue
								Status</a></td>
                        <td align="right" width="695">
                            <%--  <asp:LinkButton runat="server" ID="lnkLogout" Text="LogOut" ForeColor="Black"
                                onclick="lnkLogout_Click"></asp:LinkButton>--%>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                            <%--	<asp:Label ID="lblAppSwitch" runat="server" Text="Switch Application :"
								ForeColor="Black"></asp:Label>

							<asp:DropDownList ID="ddlAppSwitch" runat="server" Width="150px" AutoPostBack="true"
								onselectedindexchanged="ddlAppSwitch_SelectedIndexChanged">
							<asp:ListItem Value="0" Text="Select Application" Selected="True"></asp:ListItem>
							<asp:ListItem Value="1" Text="PMS" ></asp:ListItem>
							<asp:ListItem Value="2" Text="Orbit" ></asp:ListItem>
							</asp:DropDownList>--%>
						</td>
                        <td width="5">&nbsp;</td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
</table>