<%@ Register TagPrefix="uc1" TagName="header" Src="../controls/AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="ProblemPriorityMaster.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.ProblemPriorityMaster" ValidateRequest="false" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Problem Priority Master</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <script type="text/javascript" src="../Script/common.js"></script>
    <link type="text/css" rel="stylesheet" href="../css/allstyles.css">
    <script language="javascript">
			function ParametersRequired()
				{
					if(isRequire("txtProblemPriority^txtGreenResolutionHours^txtAmberResolutionHours","Problem Priority^Green Resolution Hours^Amber Resolution Hours",this.enabled))
					{
						return isInteger("txtGreenResolutionHours^txtAmberResolutionHours","Green Resolution Hours^Amber Resolution Hours")
					}
					else
					{
						return false;
					}
				}
		</script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <uc1:header ID="Header1" runat="server"></uc1:header>
                </td>
            </tr>
            <tr>
                <td height="10"></td>
            </tr>
            <tr class="header">
                <td>&nbsp;Problem Priority Master
					</td>
            </tr>
            <tr>
                <td height="10"></td>
            </tr>
            <tr class="trcolor">
                <td align="center">
                    <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label></td>
            </tr>
            <tr class="trcolor">
                <td>
                    <table width="80%" align="center" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="right">
                                <asp:LinkButton ID="lnkAddNew" runat="server" OnClick="lnkAddNew_Click">Add New Problem Priority</asp:LinkButton></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="2"></td>
            </tr>
            <tr class="trcolor">
                <td align="center">
                    <asp:Panel ID="pnlProblemPriorityTable" runat="server">
                        <asp:DataGrid ID="dgProblemPriority" CssClass="trcolor" runat="server" AutoGenerateColumns="False"
                            DataKeyField="ProblemPriorityID" Width="80%" OnItemDataBound="dgProblemPriority_Status" OnEditCommand="dgProblemPriority_Edit"
                            OnCancelCommand="dgProblemPriority_Cancel" OnUpdateCommand="dgProblemPriority_Update" OnDeleteCommand="dgProblemPriority_Delete"
                            HeaderStyle-CssClass="tableheader">
                            <HeaderStyle CssClass="tableheader"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn Visible="False" DataField="ProblemPriorityID" HeaderText="Problem Priority ID"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Problem Priority">
                                    <ItemStyle CssClass="trcolor"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"ProblemPriority") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNewProblemPriority" runat="server" CssClass="txtfield" Text='<%# DataBinder.Eval(Container, "DataItem.ProblemPriority") %>'>
											</asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Green Resolution Hours">
                                    <ItemStyle CssClass="trcolor"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"GreenResolutionHours") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNewGreenResolutionHours" runat="server" MaxLength="8" CssClass="txtfield" Text='<%# DataBinder.Eval(Container, "DataItem.GreenResolutionHours") %>'>
											</asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Amber Resolution Hours">
                                    <ItemStyle CssClass="trcolor"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"AmberResolutionHours") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNewAmberResolutionHours" runat="server" MaxLength="8" CssClass="txtfield" Text='<%# DataBinder.Eval(Container, "DataItem.AmberResolutionHours") %>'>
											</asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Status">
                                    <ItemStyle CssClass="trcolor"></ItemStyle>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"isActive") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlisActive1" runat="server" OnPreRender="SetDropDownIndex" CssClass="txtfield">
                                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                                            <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" HeaderText="Edit" CancelText="Cancel"
                                    EditText="Edit"></asp:EditCommandColumn>
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
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlAddEditProblemPriority" runat="server" Visible="False" Width="80%">
                        <table cellspacing="0" cellpadding="5" align="left" border="0">
                            <tr class="trcolor">
                                <td align="right">Problem Priority
									</td>
                                <td align="center">:
									</td>
                                <td align="left">
                                    <asp:TextBox ID="txtProblemPriority" CssClass="txtfield" runat="server" MaxLength="100"></asp:TextBox></td>
                                <td>
                                    <asp:Label ID="lblDuplicateProblemPriority" CssClass="error" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="trcolor">
                                <td align="right">Green Resolution Hours
									</td>
                                <td align="center">:
									</td>
                                <td align="left">
                                    <asp:TextBox ID="txtGreenResolutionHours" CssClass="txtfield" runat="server" MaxLength="8"></asp:TextBox></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr class="trcolor">
                                <td align="right">Amber Resolution Hours</td>
                                <td align="center">:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtAmberResolutionHours" CssClass="txtfield" runat="server" MaxLength="8"></asp:TextBox></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr class="trcolor">
                                <td align="right">Active Status</td>
                                <td align="center">:</td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlisActive" CssClass="dropdown" runat="server">
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                        <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                                    </asp:DropDownList></td>
                                <td></td>
                            </tr>
                            <tr class="trcolor">
                                <td align="center" colspan="4">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn" Text="Cancel" OnClick="btnCancel_Click"></asp:Button></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>