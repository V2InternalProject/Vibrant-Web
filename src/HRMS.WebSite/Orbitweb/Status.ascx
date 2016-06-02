<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Status.ascx.cs" Inherits="HRMS.Orbitweb.Status" %>
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <link href="../../Content/New Design/jquery.selectbox.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../../Scripts/New Design/jquery.selectbox-0.2.min.js"></script>
        <script language="javascript" type="text/javascript">
           
            function validation(StastusId) {
                if (StastusId.value == "") {
                    alert("Please enter the Status Name");
                    StastusId.focus();
                    return false;
                }
                else if (!spcharacter(StastusId)) {
                    //alert("special characters!!!");
                    return false;
                }
            }
            function spcharacter(input) {
                var txtbox = input.value;
                var iChars = "!@#$%^&*()+=-[]\\\';,./{}|\":<>?_";
                for (var i = 0; i < txtbox.length; i++) {
                    if (iChars.indexOf(txtbox.charAt(i)) != -1) {
                        alert("Please Dont enter Special Characters");
                        input.value = "";
                        return false;
                    }
                }
                return true;
            }
        </script>
        <div>
            <asp:Label runat="Server" ID="lblSuccess" SkinID="lblSuccess"></asp:Label>
            <asp:Label ID="lblError" runat="server" SkinID="lblError"></asp:Label>
        </div>
        <div>
            <asp:DataGrid runat="server" ID="grdStatus" AutoGenerateColumns="False" ShowFooter="false"
                AllowPaging="True" AllowSorting="True" OnEditCommand="grdConfigItem_EditCommand"
                OnItemCommand="grdConfigItem_ItemCommand" OnItemDataBound="grdConfigItem_ItemDataBound"
                OnPageIndexChanged="grdStatus_PageIndexChanged" CssClass="TableJqgrid" Width="100%">
                <HeaderStyle VerticalAlign="Middle" CssClass="tableHeaders" Width="400px"></HeaderStyle>
                <ItemStyle CssClass="tableRows"></ItemStyle>
                <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                <Columns>
                    <asp:TemplateColumn HeaderText="StatusID" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblStatusID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusID")%>'>
                    
                            </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblStatusID1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusID")%>'></asp:Label>
                        </EditItemTemplate>
                        <ItemStyle Width="33%" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="StatusName">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="33%" />
                        <FooterTemplate>
                            <asp:TextBox ID="txtStatusName" runat="server" MaxLength="100"></asp:TextBox>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblStatusName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusName")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtStatusName1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StatusName")%>'
                                MaxLength="100"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="IsActive">
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlFIsActive" runat="server">
                                <asp:ListItem Value="0">InActive</asp:ListItem>
                                <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="33%" />
                        <ItemTemplate>
                            <asp:Label ID="lblIsActive" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Active")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlIsActive" runat="server" SelectedIndex='<%# Convert.ToInt32(Eval("Active")) %>'>
                                <asp:ListItem Value="0">Inactive</asp:ListItem>
                                <asp:ListItem Value="1">Active</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Action">
                        <ItemStyle Width="35%" HorizontalAlign="Center"></ItemStyle>
                        <FooterTemplate>
                            <asp:LinkButton ID="btnAdd" runat="server" CommandName="AddStatus" Text="Add" />
                            &nbsp;
                            <asp:LinkButton ID="btnCancel2" runat="server" CausesValidation="False" CommandName="CancelAdd"
                                Text="Cancel" />
                        </FooterTemplate>
                        <ItemTemplate>
                            &nbsp;<asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="EditStatus"
                                Text="Edit" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="UpdateStatus" Text="Update"
                                CausesValidation="true" />&nbsp;
                            <asp:LinkButton ID="btnCancel1" runat="server" CausesValidation="False" CommandName="CancelUpdate"
                                Text="Cancel" />
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
