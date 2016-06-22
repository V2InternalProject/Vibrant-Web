<%@ Register TagPrefix="uc1" TagName="header" Src="../controls/AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="categoryMaster.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.categoryMaster" ValidateRequest="false" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Category Master</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <script src="../Script/common.js" type="text/javascript"></script>
    <link href="../css/allstyles.css" type="text/css" rel="stylesheet">
    <!--<script language=javascript>
		function setFocus()
		{
			document.getElementById("txtAddCategory").focus;
		}
		</script>-->
</head>
<body leftmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table cellspacing="0" cellpadding="0" width="100%" align="center">
            <tr>
                <td>
                    <uc1:header ID="Header1" runat="server"></uc1:header>
                </td>
            </tr>
            <tr>
                <td height="10"></td>
            </tr>
            <tr class="trcolor">
                <td class="header">&nbsp;Department Master</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblRecordMsg" runat="server" CssClass="error" Visible="False">No Records Found</asp:Label><asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td height="5"></td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="80%" align="center" border="0">
                        <tr>
                            <td align="right">
                                <asp:LinkButton ID="lbtnAddCategory" CssClass="trcolor" runat="server" OnClick="lbtnAddCategory_Click"><!--Add Department--></asp:LinkButton></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="2"></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:DataGrid ID="dgCategories" CssClass="trcolor" runat="server" PageSize="10" AllowPaging="True"
                        OnItemDataBound="dgCategories_Status" OnDeleteCommand="dgCategories_Delete" DataKeyField="CategoryID" Width="80%"
                        HorizontalAlign="Center" AutoGenerateColumns="false" OnEditCommand="dgCategories_Edit" OnCancelCommand="dgCategories_Cancel"
                        HeaderStyle-CssClass="tableheader" OnSelectedIndexChanged="dgCategories_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundColumn DataField="CategoryID" HeaderText="Category ID" Visible="False" ReadOnly="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Department">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Category") %>'>
										</asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox CssClass="txtfield" Text='<%# DataBinder.Eval (Container.DataItem, "Category")%>' runat="server" ID="lblCategory1">
										</asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label Text='<%# DataBinder.Eval (Container.DataItem, "isActive")%>' runat="server" ID="lblstatus">
										</asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList CssClass="dropdown" ID="ddlStatus" runat="server" OnPreRender="PreRenderDropdownList">
                                        <asp:ListItem Value="0">InActive</asp:ListItem>
                                        <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="View">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="View">View</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:EditCommandColumn EditText="Edit" UpdateText="Update" CancelText="Cancel" ButtonType="LinkButton" HeaderText="Edit"></asp:EditCommandColumn>
                            <asp:TemplateColumn HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:LinkButton CommandName="Delete" ID="lbtnDelete" runat="server">Delete</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle PageButtonCount="5" Mode="NumericPages" HorizontalAlign="Center"></PagerStyle>
                    </asp:DataGrid></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlAddCategory" Visible="False" runat="server" Width="80%">
                        <table id="tblAddCategory" cellspacing="0" cellpadding="5" align="left"
                            border="0">
                            <tr>
                                <td class="trcolor" align="right">Department:</td>
                                <td>
                                    <asp:TextBox ID="txtAddCategory" CssClass="txtfield" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="trcolor" align="right">Status:</td>
                                <td>
                                    <asp:DropDownList ID="ddlAddStatus" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlAddStatus_SelectedIndexChanged">
                                        <asp:ListItem Value="1" Selected>Active</asp:ListItem>
                                        <asp:ListItem Value="0">InActive</asp:ListItem>
                                    </asp:DropDownList></td>
                                <td style="height: 28px"></td>
                            </tr>
                            <tr>
                                <td class="trcolor" align="right">
                                    <asp:Label ID="lblEmployeeName" runat="server">Employee Name:</asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlEmployeeName" runat="server" CssClass="dropdown"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td align="center"></td>
                                <td align="center">
                                    <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="btn" runat="server" Text="Add"></asp:Button>
                                    <asp:Button ID="btnUpdate" OnClick="btnUpdate_Click" CssClass="btn" runat="server" Text="Update" CommandName="Update"></asp:Button>
                                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" CssClass="btn" runat="Server" Text="Cancel"></asp:Button></td>
                                <td align="center"
                                    colspan="3"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <asp:Panel ID="EmployeePanel" runat="server">
        </table>
        <table align="center">
            <tr>
                <td class="TableHeader">
                    <asp:Label ID="lblEmployeeList" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="trcolor">
                    <asp:Label ID="lblEmployeename1" runat="server"></asp:Label></td>
            </tr>
        </table>
        </asp:Panel></TABLE>
    </form>
</body>
</html>