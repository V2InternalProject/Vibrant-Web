<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="ProblemSeverityMaster.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.ProblemSeverityMaster" ValidateRequest="false" MasterPageFile="~/Views/Shared/HRMS.Master" %>

<%@ Register TagPrefix="MT" TagName="MasterHeader" Src="~/HelpDesk/controls/HelpDeskMastersTabs.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%--  old files--%>
    <script src="../lang/calendar-en.js" type="text/javascript"></script>
    <script src="../src/calendar-setup.js" type="text/javascript"></script>
    <script src="../Script/common.js" type="text/javascript"></script>
    <!-- Loading Calendar JavaScript files -->
    <script src="../utils/zapatec.js" type="text/javascript"></script>
    <script src="../src/calendar.js" type="text/javascript"></script>
    <%-- new files--%>
    <link href="../../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/Global.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/New%20Design/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#SeverityMasterTab").removeClass('tabshover').addClass('colored-border');
        });
    </script>
    <section class="HelpdeskContainer Container">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div class="MainBody">
            <div class="TabsContainer">
                <MT:MasterHeader ID="MasterHeader1" runat="server"></MT:MasterHeader>
                <section class="add-detailsdata clearfix SeverityMaster">
                    <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
                    <asp:Label ID="lblSuccessMsgs" runat="server" Visible="False" CssClass="success"></asp:Label>

                    <div class="ButtonContainer2 clearfix">
                        <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="ButtonGray" OnClick="lnkAddNew_Click">Add New Problem Severity</asp:LinkButton>
                    </div>

                    <asp:Panel ID="pnlProblemSeverityTable" runat="server">
                        <asp:DataGrid ID="dgProblemSeverity" CssClass="TableJqgrid" runat="server" AutoGenerateColumns="False"
                            DataKeyField="ProblemSeverityID" Width="100%" OnItemDataBound="dgProblemSeverity_Status" OnEditCommand="dgProblemSeverity_Edit"
                            OnCancelCommand="dgProblemSeverity_Cancel" OnUpdateCommand="dgProblemSeverity_Update" OnDeleteCommand="dgProblemSeverity_Delete">
                            <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                            <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                            <Columns>
                                <asp:BoundColumn DataField="ProblemSeverityID" HeaderText="Problem Severity ID" Visible="False"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Problem Severity">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"ProblemSeverity") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtNewProblemSeverity" CssClass="txtfield" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ProblemSeverity") %>'>
											</asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Status">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem,"isActive") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlisActive1" runat="server" OnPreRender="SetDropDownIndex" CssClass="dropdown">
                                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                                            <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                                        </asp:DropDownList>
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
                    <asp:Panel ID="pnlAddEditProblemSeverity" runat="server" Width="80%" Visible="False" CssClass="mrgnT20">
                        <div class="formrow clearfix">
                            <div class="leftcol">
                                <div class="LabelDiv">
                                    <label>Problem Severity:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtProblemSeverity" CssClass="txtfield" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="rightcol">
                                <div class="LabelDiv">
                                    <label>Active Status:</label>
                                </div>
                                <div class="InputDiv MonthYear">
                                    <asp:DropDownList ID="ddlisActive" CssClass="dropdown" runat="server" Width="150px">
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                        <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <asp:Label ID="lblDuplicateProblemSeverity" CssClass="error" runat="server"></asp:Label>
                        <div class="clearfix">
                            <asp:Button ID="btnSubmit" runat="server" CssClass="ButtonGray mrgnT10" Text="Submit" OnClick="btnSubmit_Click"></asp:Button>
                            <asp:Button ID="btnCancel" runat="server" CssClass="ButtonGray mrgnT10" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
                        </div>
                    </asp:Panel>
                </section>
            </div>
        </div>
    </section>
</asp:Content>