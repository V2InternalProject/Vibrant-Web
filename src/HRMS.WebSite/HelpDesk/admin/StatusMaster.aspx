<%@ Register TagPrefix="uc1" TagName="header" Src="../controls/AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="StatusMaster.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.StatusMaster" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Status Master</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <script type="text/javascript" src="../Script/common.js"></script>
    <link type="text/css" rel="stylesheet" href="../css/allstyles.css">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr class="trcolor">
                <td>
                    <uc1:header ID="Header1" runat="server"></uc1:header>
                </td>
            </tr>
            <tr>
                <td class="header" height="10"></td>
            </tr>
            <tr class="trcolor">
                <td class="header">&nbsp;Status Master
					</td>
            </tr>
            <tr class="trcolor">
                <td align="center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label></td>
            </tr>
            <tr class="trcolor">
                <td>
                    <table width="80%" align="center" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td align="right">
                                <asp:LinkButton ID="lnkAddNew" runat="server" OnClick="lnkAddNew_Click">Add New</asp:LinkButton></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlStatusTable" runat="server">
                        <asp:DataGrid ID="dgStatus" CssClass="trcolor" runat="server" OnDeleteCommand="dgStatus_Delete"
                            OnUpdateCommand="dgStatus_Update" OnCancelCommand="dgStatus_Cancel" OnEditCommand="dgStatus_Edit"
                            Width="80%" DataKeyField="StatusID" AutoGenerateColumns="False" CellPadding="5">
                            <HeaderStyle CssClass="tableheader"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="StatusID" HeaderText="StatusID" Visible="False"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Status">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"Status") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNewStatus" runat="server" CssClass="txtfield" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'>
											</asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:EditCommandColumn EditText="Edit" UpdateText="Update" CancelText="Cancel" ButtonType="LinkButton"
                                    HeaderText="Edit"></asp:EditCommandColumn>
                                <asp:TemplateColumn HeaderText="Delete">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </asp:Panel>
                </td>
            </tr>
            <tr class="trcolor">
                <td align="center">
                    <asp:Panel ID="pnlAddEditStatus" runat="server" Width="80%" Visible="False">
                        <table cellspacing="0" cellpadding="5" align="left" border="0">
                            <tr class="trcolor">
                                <td align="right">Status:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtStatus" CssClass="txtfield" runat="server" MaxLength="100"></asp:TextBox></td>
                                <td>
                                    <asp:Label ID="lblDuplicateStatus" CssClass="error" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="trcolor">
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn" Text="Cancel" OnClick="btnCancel_Click"></asp:Button></td>
                                <td></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>