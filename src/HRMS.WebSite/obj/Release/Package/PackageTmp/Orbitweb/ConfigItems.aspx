<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master" AutoEventWireup="true"
    CodeBehind="ConfigItems.aspx.cs" Inherits="HRMS.Orbitweb.ConfigItems" %>

<%@ Register Src="~/Orbitweb/OrbitMastersTabs.ascx" TagPrefix="RT" TagName="OrbitMastersTabs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <ajaxToolkit:ToolkitScriptManager EnablePartialRendering="true" runat="Server" ID="ScriptManager1" />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <script language="javascript" type="text/javascript">
                function pageLoad() {
                    var name = "<%=ViewState["AdminMaster"]%>";
              if (name == 'ConfigSetting')
                  $('#ConfigSettingsTab').addClass('colored-border');
          }
          function validation(ConfigItemValue) {
              if (ConfigItemValue.value == "") {
                  alert("Please enter the Config Item Value");
                  return false;
              }
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
                    <RT:OrbitMastersTabs ID="OrbitMastersTabs1" runat="server"></RT:OrbitMastersTabs>
                    <div class="clearfix">
                        <asp:Label runat="Server" ID="lblSuccess" SkinID="lblSuccess"></asp:Label>
                        <asp:Label ID="lblError" runat="server" SkinID="lblError"></asp:Label>
                    </div>
                    <div class="clearfix mrgnT25">
                        <asp:DataGrid ID="grdConfigItem" runat="server" PageSize="1000" OnEditCommand="grdConfigItem_EditCommand"
                            AutoGenerateColumns="False" AllowPaging="True" OnItemCommand="grdConfigItem_ItemCommand"
                            OnItemDataBound="grdConfigItem_ItemDataBound" OnPageIndexChanged="grdConfigItem_PageIndexChanged"
                            CssClass="TableJqgrid" Width="96%">
                            <EditItemStyle VerticalAlign="Middle" HorizontalAlign="Left"></EditItemStyle>
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" CssClass="tableRows"></ItemStyle>
                            <HeaderStyle VerticalAlign="Middle" CssClass="tableHeaders"></HeaderStyle>
                            <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                            <FooterStyle CssClass="tableRows"></FooterStyle>
                            <Columns>
                                <asp:TemplateColumn HeaderText="ConfigItem ID" Visible="False">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblConfigItemID" Text='<%# DataBinder.Eval(Container.DataItem, "ConfigItemID") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label runat="server" ID="lblConfigItemID2" Text='<%# DataBinder.Eval(Container.DataItem, "ConfigItemID") %>'>
                                        </asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="ConfigItem Name" ItemStyle-Width="18%">
                                    <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblConfigItemName" Text='<%# DataBinder.Eval(Container.DataItem, "ConfigItemName") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="ConfigItem Value" ItemStyle-Width="32%">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblConfigItemValue" Text='<%# DataBinder.Eval(Container.DataItem, "ConfigItemValue") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtConfigItemValue1" MaxLength="100" runat="server" Height="28px"
                                            Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "ConfigItemValue") %>'>
                                        </asp:TextBox>
                                        <asp:DropDownList ID="DDlHours" runat="server" AutoPostBack="false" CausesValidation="True"
                                            Visible="false">
                                            <asp:ListItem Value="0">00 </asp:ListItem>
                                            <asp:ListItem Value="1">01</asp:ListItem>
                                            <asp:ListItem Value="2">02 </asp:ListItem>
                                            <asp:ListItem Value="3">03</asp:ListItem>
                                            <asp:ListItem Value="4">04 </asp:ListItem>
                                            <asp:ListItem Value="5">05 </asp:ListItem>
                                            <asp:ListItem Value="6">06 </asp:ListItem>
                                            <asp:ListItem Value="7">07 </asp:ListItem>
                                            <asp:ListItem Value="8">08 </asp:ListItem>
                                            <asp:ListItem Value="9">09</asp:ListItem>
                                            <asp:ListItem Value="10">10 </asp:ListItem>
                                            <asp:ListItem Value="11">11 </asp:ListItem>
                                            <asp:ListItem Value="12">12 </asp:ListItem>
                                            <asp:ListItem Value="13">13</asp:ListItem>
                                            <asp:ListItem Value="14">14 </asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="16">16 </asp:ListItem>
                                            <asp:ListItem Value="17">17 </asp:ListItem>
                                            <asp:ListItem Value="18">18 </asp:ListItem>
                                            <asp:ListItem Value="19">19 </asp:ListItem>
                                            <asp:ListItem Value="20">20 </asp:ListItem>
                                            <asp:ListItem Value="21">21 </asp:ListItem>
                                            <asp:ListItem Value="22">22 </asp:ListItem>
                                            <asp:ListItem Value="23">23 </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlMins" runat="server" AutoPostBack="false" CausesValidation="True"
                                            Visible="false">
                                            <asp:ListItem Value="0">00 </asp:ListItem>
                                            <asp:ListItem Value="1">01 </asp:ListItem>
                                            <asp:ListItem Value="2">02</asp:ListItem>
                                            <asp:ListItem Value="3">03 </asp:ListItem>
                                            <asp:ListItem Value="4">04 </asp:ListItem>
                                            <asp:ListItem Value="5">05</asp:ListItem>
                                            <asp:ListItem Value="6">06 </asp:ListItem>
                                            <asp:ListItem Value="7">07</asp:ListItem>
                                            <asp:ListItem Value="8">08 </asp:ListItem>
                                            <asp:ListItem Value="9">09</asp:ListItem>
                                            <asp:ListItem Value="10">10 </asp:ListItem>
                                            <asp:ListItem Value="11">11</asp:ListItem>
                                            <asp:ListItem Value="12">12 </asp:ListItem>
                                            <asp:ListItem Value="13">13</asp:ListItem>
                                            <asp:ListItem Value="14">14 </asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="16">16 </asp:ListItem>
                                            <asp:ListItem Value="17">17</asp:ListItem>
                                            <asp:ListItem Value="18">18 </asp:ListItem>
                                            <asp:ListItem Value="19">19</asp:ListItem>
                                            <asp:ListItem Value="20">20 </asp:ListItem>
                                            <asp:ListItem Value="21">21</asp:ListItem>
                                            <asp:ListItem Value="22">22 </asp:ListItem>
                                            <asp:ListItem Value="23">23</asp:ListItem>
                                            <asp:ListItem Value="24">24 </asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="26">26 </asp:ListItem>
                                            <asp:ListItem Value="27">27</asp:ListItem>
                                            <asp:ListItem Value="28">28 </asp:ListItem>
                                            <asp:ListItem Value="29">29</asp:ListItem>
                                            <asp:ListItem Value="30">30 </asp:ListItem>
                                            <asp:ListItem Value="31">31</asp:ListItem>
                                            <asp:ListItem Value="32">32 </asp:ListItem>
                                            <asp:ListItem Value="33">33</asp:ListItem>
                                            <asp:ListItem Value="34">34 </asp:ListItem>
                                            <asp:ListItem Value="35">35</asp:ListItem>
                                            <asp:ListItem Value="36">36 </asp:ListItem>
                                            <asp:ListItem Value="37">37</asp:ListItem>
                                            <asp:ListItem Value="38">38 </asp:ListItem>
                                            <asp:ListItem Value="39">39 </asp:ListItem>
                                            <asp:ListItem Value="40">40 </asp:ListItem>
                                            <asp:ListItem Value="41">41 </asp:ListItem>
                                            <asp:ListItem Value="42">42 </asp:ListItem>
                                            <asp:ListItem Value="43">43 </asp:ListItem>
                                            <asp:ListItem Value="44">44 </asp:ListItem>
                                            <asp:ListItem Value="45">45 </asp:ListItem>
                                            <asp:ListItem Value="46">46 </asp:ListItem>
                                            <asp:ListItem Value="47">47 </asp:ListItem>
                                            <asp:ListItem Value="48">48 </asp:ListItem>
                                            <asp:ListItem Value="49">49 </asp:ListItem>
                                            <asp:ListItem Value="50">50 </asp:ListItem>
                                            <asp:ListItem Value="51">51 </asp:ListItem>
                                            <asp:ListItem Value="52">52 </asp:ListItem>
                                            <asp:ListItem Value="53">53 </asp:ListItem>
                                            <asp:ListItem Value="54">54 </asp:ListItem>
                                            <asp:ListItem Value="55">55 </asp:ListItem>
                                            <asp:ListItem Value="56">56 </asp:ListItem>
                                            <asp:ListItem Value="57">57 </asp:ListItem>
                                            <asp:ListItem Value="58">58 </asp:ListItem>
                                            <asp:ListItem Value="59">59 </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" Visible="false"
                                            runat="server" ID="imgbtnSearchToDate1" ImageAlign="Middle" CssClass="ui-datepicker-trigger" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate1" runat="server" TargetControlID="txtConfigItemValue1"
                                            PopupButtonID="imgbtnSearchToDate1" />
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="ConfigItem Description" ItemStyle-Width="27%">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblConfigItemDescription" Text='<%# DataBinder.Eval(Container.DataItem, "ConfigItemDescription") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtConfigItemDescription1" TextMode="MultiLine" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ConfigItemDescription") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Action" ItemStyle-Width="23%">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbutEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkbutUpdate" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>&nbsp;

                                        <asp:LinkButton ID="lnkbutCancel" runat="server" Text="Cancel" CommandName="btnCancel"
                                            CausesValidation="false"></asp:LinkButton>
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