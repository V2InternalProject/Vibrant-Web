<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="../controls/AdminHeader.ascx" %>

<%@ Page Language="c#" CodeBehind="subCategoryMaster.aspx.cs" AutoEventWireup="True" Inherits="V2.Helpdesk.web.admin.subCategoryMaster" MasterPageFile="~/Views/Shared/HRMS.Master" %>

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
            $("#CategoryMasterTab").removeClass('tabshover').addClass('colored-border');
        });
    </script>
    <section class="HelpdeskContainer Container">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div class="MainBody">
            <div class="TabsContainer">
                <MT:MasterHeader ID="MasterHeader1" runat="server"></MT:MasterHeader>
                <section class="add-detailsdata clearfix CategoryMaster">
                    <asp:Label ID="lblRecordMsg" runat="server" Visible="True" CssClass="error">No Records Found</asp:Label>
                    <asp:Label ID="lblError" runat="server" Visible="False" CssClass="error"></asp:Label>
                    <asp:Label ID="lblSuccessMsgs" runat="server" Visible="False" CssClass="success"></asp:Label>

                    <div class="ButtonContainer2 clearfix">
                        <asp:LinkButton ID="lbtnAddSubCategory" CssClass="ButtonGray" runat="server" OnClick="lbtnAddSubCategory_Click">Add Category</asp:LinkButton>
                    </div>

                    <asp:DataGrid ID="dgSubCategories" CssClass="TableJqgrid" runat="server" Width="100%" HorizontalAlign="Center"
                        AutoGenerateColumns="false" DataKeyField="SubCategoryID" OnItemDataBound="dgSubCategories_Status" OnDeleteCommand="dgSubCategories_Delete"
                        OnEditCommand="dgSubCategories_Edit" OnCancelCommand="dgSubCategories_Cancel" OnUpdateCommand="dgSubCategories_Update"
                        HeaderStyle-CssClass="TableHeader" AllowPaging="True" PageSize="10" OnPageIndexChanged="dgSubCategories_PageIndexChanged"
                        OnItemCommand="dgSubCategories_ItemCommand">
                        <ItemStyle CssClass="tableRows" HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                        <HeaderStyle Wrap="True" CssClass="tableHeaders"></HeaderStyle>
                        <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                        <Columns>
                            <asp:BoundColumn DataField="SubCategoryID" ReadOnly="True" HeaderText="Category ID"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CategoryID" ReadOnly="True" HeaderText="Category ID"></asp:BoundColumn>
                            <%--<asp:BoundColumn DataField="Category" ReadOnly="True" HeaderText="Category ID"></asp:BoundColumn>--%>
                            <asp:TemplateColumn HeaderText="Department">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem,"Category") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlCategory" CssClass="dropdown" runat="server" OnPreRender="PreRenderddlCategory"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Category">
                                <ItemTemplate>
                                    <asp:Label ID="SubCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SubCategory") %>'>
										</asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSubCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SubCategory")%>' CssClass="txtfield">
										</asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label Text='<%# DataBinder.Eval (Container.DataItem, "isActive")%>' runat="server" ID="lblstatus">
										</asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="dropdown" OnPreRender="PreRenderddlStatus">
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
                            <asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" HeaderText="Edit" CancelText="Cancel"
                                EditText="Edit"></asp:EditCommandColumn>
                            <%--<asp:TemplateColumn HeaderText="Delete">
									<ItemTemplate>
										<asp:LinkButton id="lbtnDelete" Runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');">Delete</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>--%>
                        </Columns>
                    </asp:DataGrid>
                    <asp:Panel ID="pnlAddSubCategory" Visible="False" runat="server" Width="100%" CssClass="mrgnT20">
                        <div class="formrow clearfix">
                            <div class="leftcol">
                                <div class="LabelDiv">
                                    <label>Category:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:TextBox ID="txtAddSubCategory" CssClass="txtfield" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="rightcol">
                                <div class="LabelDiv">
                                    <label>Department:</label>
                                </div>
                                <div class="InputDiv MonthYear">
                                    <asp:DropDownList ID="ddlAddCategory" runat="server" CssClass="dropdown" Width="150px"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="formrow clearfix">
                            <div class="leftcol">
                                <div class="LabelDiv">
                                    <label>Status:</label>
                                </div>
                                <div class="InputDiv">
                                    <asp:DropDownList ID="ddlAddStatus" runat="server" CssClass="dropdown" Width="150px">
                                        <asp:ListItem Value="1">InActive</asp:ListItem>
                                        <asp:ListItem Value="0" Selected="True">Active</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="False">Please Enter a Category</asp:Label>
                        <asp:Label ID="lblCategory" runat="server" CssClass="error" Visible="False">Please Enter a Department</asp:Label>

                        <div class="clearfix">
                            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="ButtonGray mrgnT10" runat="server" Text="Submit"></asp:Button>
                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" CssClass="ButtonGray mrgnT10" runat="Server" Text="Cancel"></asp:Button>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="EmployeePanel" runat="server" CssClass="mrgnT20">
                        <div>
                            <asp:Label ID="lblEmployeeList" runat="server" CssClass="suffix"></asp:Label></div>

                        <div class="LabelDiv">
                            <asp:Label ID="lblEmployeename" runat="server"></asp:Label></div>
                    </asp:Panel>
                </section>
            </div>
        </div>
    </section>
</asp:Content>