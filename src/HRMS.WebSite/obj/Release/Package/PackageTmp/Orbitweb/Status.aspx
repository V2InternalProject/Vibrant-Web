<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="true"
    CodeBehind="Status.aspx.cs" Inherits="HRMS.Orbitweb.Status" %>

<%@ Register Src="~/Orbitweb/OrbitMastersTabs.ascx" TagPrefix="RT" TagName="OrbitMastersTabs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <link href="../../Content/New Design/jquery.selectbox.css" type="text/css" rel="stylesheet" />
            <script type="text/javascript" src="../../Scripts/New Design/jquery.selectbox-0.2.min.js"></script>
            <script language="javascript" type="text/javascript">
           function pageLoad() {
        var name = "<%=ViewState["AdminMaster"]%>";
        if(name == 'Status')
        $('#StatusMasterTab').addClass('colored-border');
        }
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
            <section class="Container AttendanceContainer">
                <div class="FixedHeader">
                    <div class="clearfix">
                        <h2 class="MainHeading">Administration</h2>
                    </div>
                    <nav class="sub-menu-colored">
                        <a href="AdminApproval.aspx">Admin Approval</a> <a href="LeaveTransaction.aspx">Leave
                            Transaction</a> <a href="BulkEntries.aspx">Bulk Entries</a> <a href="AdminLeaveApplicationForm.aspx">Admin Leave Application</a><%--<a href="StartWorkflows.aspx">Manage Processes</a>--%>
                        <a href="HolidayList.aspx" class="selected">Masters</a>
                    </nav>
                </div>
                <div class="MainBody Admin">
                    <RT:OrbitMastersTabs ID="OrbitMastersTabs" runat="server"></RT:OrbitMastersTabs>
                    <div class="clearfix">
                        <asp:Label runat="Server" ID="lblSuccess" SkinID="lblSuccess"></asp:Label>
                        <asp:Label ID="lblError" runat="server" SkinID="lblError"></asp:Label>
                    </div>
                    <div class="clearfix mrgnT25">
                        <asp:DataGrid runat="server" ID="grdStatus" AutoGenerateColumns="False" ShowFooter="false"
                            AllowPaging="True" AllowSorting="True" OnEditCommand="grdConfigItem_EditCommand"
                            OnItemCommand="grdConfigItem_ItemCommand" OnItemDataBound="grdConfigItem_ItemDataBound"
                            OnPageIndexChanged="grdStatus_PageIndexChanged" CssClass="TableJqgrid" Width="96%">
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
                                        &nbsp;<asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="EditStatus" Enabled="false"
                                            Text="Edit" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="btnUpdate" runat="server" CommandName="UpdateStatus" Text="Update" Enabled="false"
                                            CausesValidation="true" />&nbsp;

                                        <asp:LinkButton ID="btnCancel1" runat="server" CausesValidation="False" CommandName="CancelUpdate" Enabled="false"
                                            Text="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>