<%@ Page Language="c#" CodeBehind="MoveIssue.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.MoveIssue" %>

<%@ Register TagPrefix="uc1" TagName="header" Src="../controls/AdminHeader.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>View All Issues</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../css/allstyles.css" type="text/css" rel="stylesheet">
    <script src="../Script/common.js" type="text/javascript"></script>
    <script lang="javascript">
        function validateEmployee() {

            var PageSize = 0
            for (i = 0; i < document.forms[0].elements.length; i++) {
                var elm = document.forms[0].elements[i];
                var Objname = elm.name;
                if (elm.type == 'checkbox') {
                    var str = Objname.indexOf("chkIsuueIdAll")
                    if (str <= 0) {
                        //alert(elm.checked)
                        if (elm.checked == true) {
                            //alert("Hello3")
                            //alert(PageSize)
                            //break;
                            PageSize++;
                        }
                        else {

                            //alert(PageSize)
                        }
                    }
                }
            }
            // alert(PageSize);
            if (PageSize == 5 || PageSize == 0) {
                alert("Please select the Issue to Move");
                return false;
            }
            var ddlIssueMoveto = document.getElementById("ddlIssueMoveto").value;
            if (ddlIssueMoveto == 0) {
                alert("Please select Employee Name ");
                return false;
            }

        }

        function validateCategory() {
            var ddlCategory = document.getElementById("ddlCategory").value;
            if (ddlCategory == 0) {
                alert("Please select Category ");
                return false;
            }
        }
		</script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
            <tr>
                <td>
                    <uc1:header ID="Header1" runat="server"></uc1:header>
                </td>
            </tr>
            <tr>
                <td class="header" height="10"></td>
            </tr>
            <tr class="trcolor">
                <td class="header">&nbsp;View&nbsp;All&nbsp;Issues
					</td>
            </tr>
            <tr class="trcolor">
                <td align="center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
                    <asp:Label ID="lblSuccessMessage" Visible="false" runat="server" CssClass="success"></asp:Label></td>
            </tr>
            <tr>
                <td>&nbsp;

                    <table cellspacing="3" width="50%" align="center">
                        <tbody>
                            <tr>
                                <td class="trcolor" align="left" width="25%">
                                    <asp:Label ID="lblIssueMoveFrom" runat="server">Issue Move From</asp:Label></td>
                                <td class="trcolor">
                                    <asp:DropDownList ID="ddlIssueMoveFrom" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlIssueMoveFrom_SelectedIndexChanged"></asp:DropDownList>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="trcolor" align="left" width="25%">
                                    <asp:Label ID="lblCategory" runat="server">Category</asp:Label></td>
                                <td class="trcolor">
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="dropdown"></asp:DropDownList><asp:Button ID="btnGo" runat="server" CssClass="btn" Text="GO" OnClick="btnGo_Click"></asp:Button></td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:DataGrid ID="dgViewIssues" runat="server" CssClass="trcolor" OnItemCommand="dgViewIssues_ItemCommand"
                        DataKeyField="ReportIssueID" AutoGenerateColumns="False" AllowPaging="True" Width="80%" ItemStyle-VerticalAlign="Top"
                        AlternatingItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left" AlternatingItemStyle-HorizontalAlign="Left"
                        CellPadding="5" OnPageIndexChanged=" dgViewIssues_PageIndexChanged">
                        <AlternatingItemStyle HorizontalAlign="Left" VerticalAlign="Top"></AlternatingItemStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                        <HeaderStyle Wrap="False" CssClass="tableheader"></HeaderStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <input id="chkIsuueIdAll" name="chkIsuueIdAll" type="checkbox" onclick="JavaScript: CheckAllDataGridCheckBoxes('chkIsuue', this.checked);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkIsuue" onclick="JavaScript:CheckDataGridCheckBox('chkIsuue',this.checked);"
                                        runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="False" HeaderText="IssueAssignmentID">
                                <HeaderStyle CssClass="bodytxt"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="IssueId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ReportIssueId") %>'>
										</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Issue ID">
                                <HeaderStyle Wrap="False" Width="8%" CssClass="bodytxt"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblReportIssueID" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.ReportIssueID")%>'>
										</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="False" HeaderText="IssueAssignment ID">
                                <HeaderStyle CssClass="bodytxt"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueAssignmentID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ReportIssueID") %>'>
										</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="Category" ReadOnly="True" HeaderText="Department">
                                <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SubCategory" ReadOnly="True" HeaderText="Category">
                                <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ProblemSeverity" ReadOnly="True" HeaderText="Severity">
                                <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" ReadOnly="True">
                                <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ReportIssueDate" ReadOnly="True" HeaderText="Reported On">
                                <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Description" ReadOnly="True" HeaderText="Description">
                                <HeaderStyle HorizontalAlign="Center" Width="31%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Status ID">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatusID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusId") %>'>
										</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NumericPages" HorizontalAlign="Center"></PagerStyle>
                    </asp:DataGrid></td>
            </tr>
        </table>
        <table cellspacing="3" width="50%" align="center">
            <tr>
                <td class="trcolor" align="left" width="25%"></td>
                <td class="trcolor"></td>
            </tr>
            <tr>
                <td class="trcolor" align="left" width="25%">
                    <asp:Label ID="lblIssuemoveTo" runat="server">Issue Move To</asp:Label></td>
                <td class="trcolor">
                    <asp:DropDownList ID="ddlIssueMoveto" runat="server" CssClass="dropdown"></asp:DropDownList>&nbsp;

                    <asp:Button ID="btnMove" runat="server" CssClass="btn" Text="Move" OnClick="btnMove_Click"></asp:Button></td>
            </tr>
        </table>
    </form>
</body>
</html>