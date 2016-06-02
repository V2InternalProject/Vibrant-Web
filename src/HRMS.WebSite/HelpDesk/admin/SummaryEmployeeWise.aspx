<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>
<%@ Page language="c#" Codebehind="SummaryEmployeeWise.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.SummaryEmployeeWise" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="../controls/header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SummaryEmployeeWise</title>
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../css/allstyles.css" type="text/css" rel="stylesheet">
		<LINK href="../themes/aqua.css" rel="stylesheet">
		<!-- Loading Theme file(s) -->
		<!-- Loading Calendar JavaScript files -->
		<script src="../utils/zapatec.js" type="text/javascript"></script>
		<script src="../src/calendar.js" type="text/javascript"></script>
		<!-- Loading language definition file -->
		<script src="../lang/calendar-en.js" type="text/javascript"></script>
		<script src="../src/calendar-setup.js" type="text/javascript"></script>
		<script src="../Script/common.js" type="text/javascript"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<tr>
					<td>
						<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader></td>
				</tr>
				<TR>
					<TD class="header" height="10"></TD>
				</TR>
				<tr class="trcolor">
					<td class="header">&nbsp;View&nbsp;All&nbsp;Summary&nbsp;Report
					</td>
				</tr>
				<tr class="trcolor">
					<td align="center"><asp:label id="lblMsg" CssClass="error" Runat="server"></asp:label></td>
				</tr>
				<tr>
					<td>&nbsp;
						<table cellSpacing="3" width="50%" align="center">
							<TBODY>
								<tr>
								<TR>
									<TD class="trcolor" align="right" width="25%"><asp:RequiredFieldValidator InitialValue="0" ID="rfvEmp" runat="server" ControlToValidate="ddlEmployees" ErrorMessage="Select Employee">*</asp:RequiredFieldValidator>Select 
										Employee:</TD>
									<td class="trcolor"><asp:dropdownlist id="ddlEmployees" runat="server" CssClass="dropdown"></asp:dropdownlist></td>
								</TR>
								<tr>
									<TD class="trcolor" align="right" width="25%"><asp:RequiredFieldValidator ID="rfvfromDate" ErrorMessage="Select From Date " ControlToValidate="txtFromDate"
											runat="server">*</asp:RequiredFieldValidator>From Date:</TD>
									<TD vAlign="middle"><asp:textbox id="txtFromDate" CssClass="txt" Runat="server" ReadOnly="True"></asp:textbox>&nbsp;
										<asp:imagebutton id="ibtnFromDateCalendar" Runat="server" ImageUrl="../Images/calender.jpg"></asp:imagebutton>
										<SCRIPT type="text/javascript">//<![CDATA[
														Zapatec.Calendar.setup({
														inputField     :    "txtFromDate",           
														ifFormat       :    "%m/%d/%Y",
														showsTime      :    false,
														button         :    "ibtnFromDateCalendar",        
														step           :    1,
														firstDay          : 1,
														weekNumbers       : false,
														showOthers        : true,
														electric          : false
														
														});	
										</SCRIPT>
										<!-- Below link is put just to avoid the messagebox on page load 
											saying that it is necessary to mention a link to the zapatech site--><a href="http://www.zapatec.com/website/main/products/prod1/"></a></TD>
								</tr>
								<TR>
									<TD class="trcolor" align="right"><asp:RequiredFieldValidator ID="rfvTodate" ErrorMessage="Select To Date " ControlToValidate="txtToDate" runat="server">*</asp:RequiredFieldValidator>To 
										Date:</TD>
									<TD vAlign="middle"><asp:textbox id="txtToDate" CssClass="txt" Runat="server" ReadOnly="True"></asp:textbox>&nbsp;
										<asp:imagebutton id="ibtnFromToCalendar" Runat="server" ImageUrl="../Images/calender.jpg"></asp:imagebutton>&nbsp;<asp:button id="btnSubmit" runat="server" CssClass="btn" Text="Submit" onclick="btnSubmit_Click"></asp:button>
										<SCRIPT type="text/javascript">//<![CDATA[
														Zapatec.Calendar.setup({
														inputField     :    "txtToDate",           
														ifFormat       :    "%m/%d/%Y",
														showsTime      :    false,
														button         :    "ibtnFromToCalendar",        
														step           :    1,
														firstDay          : 1,
														weekNumbers       : false,
														showOthers        : true,
														electric          : false
														
														});	
										</SCRIPT>
									</TD>
								</TR>
							</TBODY>
						</table>
					</td>
				</tr>
				<TR>
					<TD align="center" height="5">
						<asp:CompareValidator ID="CmpDate" runat="server" Visible="True" ControlToValidate="txtToDate" ErrorMessage="The From Date should not be greater than the To Date"
							ControlToCompare="txtFromDate" Operator="GreaterThanEqual" Type="Date" Display="None"></asp:CompareValidator>
					</TD>
				</TR>
				<tr>
					<td align="center"><asp:datagrid class="gridlink" id="dgEmpSummaryReport" CssClass="trcolor" Runat="server" AutoGenerateColumns="True"
							CellPadding="5" AllowPaging="True" Width="90%" PageSize="10" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left"
							AlternatingItemStyle-VerticalAlign="Top" AlternatingItemStyle-HorizontalAlign="Left">
							<AlternatingItemStyle HorizontalAlign="Left" VerticalAlign="Top"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
							<HeaderStyle Wrap="False" CssClass="tableheader"></HeaderStyle>
							<Columns>
							</Columns>
							<PagerStyle PageButtonCount="5" Mode="NumericPages" HorizontalAlign="Center"></PagerStyle>
						</asp:datagrid></td>
				</tr>
			</table>
			<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary>
		</form>
	</body>
</HTML>
