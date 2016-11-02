<%@ Page Title="Holiday Lists" Language="C#" MasterPageFile="~/Views/Shared/HRMS.Master"
    AutoEventWireup="true" CodeBehind="HolidayList.aspx.cs" Inherits="HRMS.Orbitweb.HolidayList" %>

<%@ Register Src="~/Orbitweb/OrbitMastersTabs.ascx" TagPrefix="RT" TagName="OrbitMastersTabs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Content/New%20Design/travel.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            var name = "<%=ViewState["AdminMaster"]%>";
           if (name == 'HolidayList')
               $('#HolidayListTab').addClass('colored-border');
       }

       function validation(HolidaysDescription, txtHolidayDate) {

           var HolidaysDescription = document.getElementById(HolidaysDescription).value;
           var txtHolidayDate = document.getElementById(txtHolidayDate).value;
           if (txtHolidayDate == "") {
               alert("Please enter the Holidays Date");

               return false;
           }
           else if (HolidaysDescription == "") {
               alert("Please enter the Holidays Description");

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
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" EnablePartialRendering="true">
    </ajaxToolkit:ToolkitScriptManager>
    <section class="Container AttendanceContainer">
        <div class="FixedHeader">
            <div class="clearfix">
                <h2 class="MainHeading">Administration</h2>
            </div>
            <nav class="sub-menu-colored">
                <a href="AdminApproval.aspx">Admin Approval</a> <a href="LeaveTransaction.aspx">Leave
                    Transaction</a> <a href="BulkEntries.aspx">Bulk Entries</a> <a href="AdminLeaveApplicationForm.aspx">Admin Leave Application</a>
                <a href="HolidayList.aspx" class="selected">Masters</a>
            </nav>
        </div>
        <div class="MainBody Admin">
            <RT:OrbitMastersTabs ID="OrbitMastersTabs1" runat="server"></RT:OrbitMastersTabs>
            <div class="clearfix">
                <div class="clearfix">
                    <asp:Label runat="Server" ID="lblSuccess" SkinID="lblSuccess"></asp:Label>
                    <asp:Label ID="lblError" runat="server" SkinID="lblError"></asp:Label>
                </div>
                <div class="FormContainerBox clearfix">
                    <p class="OrbitNote">
                        * Previous date entry is not Added or Edited
                    </p>
                    <div class="leftcol">
                        <div class="formrow clearfix selectYr">
                            <div class="LabelDiv">
                                <asp:Label ID="lblSelectYear" Text="Select Year:" runat="server"></asp:Label>
                            </div>
                            <div class="InputDiv">
                                <asp:DropDownList ID="DDlYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDlYear_SelectedIndexChanged"
                                    CausesValidation="True" Enabled="false">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:DataGrid ID="grdHolidayList" runat="server" ShowFooter="True" PageSize="1000"
                OnEditCommand="grdHolidayList_EditCommand" AutoGenerateColumns="False" AllowPaging="True"
                Width=" 96%" OnItemCommand="grdHolidayList_ItemCommand" OnItemDataBound="grdHolidayList_ItemDataBound"
                CssClass="TableJqgrid mrgnT20">
                <EditItemStyle VerticalAlign="Middle" HorizontalAlign="Left"></EditItemStyle>
                <ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" CssClass="tableRows"></ItemStyle>
                <HeaderStyle CssClass="tableHeaders" />
                <PagerStyle CssClass="gridPager" HorizontalAlign="Right" Mode="NumericPages" />
                <FooterStyle CssClass="tableRows"></FooterStyle>
                <Columns>
                    <asp:TemplateColumn HeaderText="Holidays ID" Visible="False">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblHolidayID" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayID") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                        <FooterTemplate>
                            <asp:Label ID="lblHolidayID1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayID") %>'>
                            </asp:Label>
                        </FooterTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="lblHolidayID2" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayID") %>'>
                            </asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Holidays Date" ItemStyle-Width="36%">
                        <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblHolidayDate" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayDate") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtHolidaydate" runat="server" Height="28px" MaxLength="50" Width="100px" Enabled="false"
                                Text='<%# DataBinder.Eval(Container.DataItem, "HolidayDate") %>'>
                            </asp:TextBox>
                            <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server"
                                ID="imgbtnSearchToDate1" ImageAlign="Middle" CssClass="ui-datepicker-trigger" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate1" runat="server" TargetControlID="txtHolidayDate"
                                PopupButtonID="imgbtnSearchToDate1" />
                        </FooterTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtHolidayDate1" runat="server" MaxLength="50" Height="28px" Width="100px"
                                Text='<%# DataBinder.Eval(Container.DataItem, "HolidayDate") %>'>
                            </asp:TextBox>
                            <asp:ImageButton ImageUrl="~/images/New Design/calender-icon.png" runat="server"
                                ID="imgbtnSearchToDate2" ImageAlign="Middle" CssClass="ui-datepicker-trigger" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtenderSearchToDate2" runat="server" TargetControlID="txtHolidayDate1"
                                PopupButtonID="imgbtnSearchToDate2" />
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Holidays Description">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblHolidayDescriptio" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayDescription") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                        <FooterTemplate>
                            <asp:TextBox ID="txtHolidayDescription" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                        </FooterTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtHolidayDescription1" runat="server" Width="90%" MaxLength="50"
                                Text='<%# DataBinder.Eval(Container.DataItem, "HolidayDescription") %>'>
                            </asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Holidays Location">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblHolidayLocationID" Text='<%# DataBinder.Eval(Container.DataItem, "OfficeLocation") %>' Visible="false"></asp:Label>
                            <asp:Label runat="server" ID="lblHolidayLocation" Text='<%# DataBinder.Eval(Container.DataItem, "Location") %>'></asp:Label>
                            <%--<asp:DropDownList runat="server" ID="ddlHolidayLocation" Width="90%" MaxLength="50">
                            </asp:DropDownList>--%>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlHolidayLocation1" runat="server" Width="90%" MaxLength="50" Enabled="false" ></asp:DropDownList>
                        </FooterTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlHolidayLocation2" runat="server" Width="90%" MaxLength="50">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="IsHolidayForShift">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIsForShift" runat="server" Checked='<%# Convert.ToBoolean( DataBinder.Eval(Container.DataItem, "IsHolidayForShift") )%>' />
                            <asp:Label runat="server" AssociatedControlID="chkIsForShift" class="LabelForCheckbox"></asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle"></FooterStyle>
                        <FooterTemplate>
                            <asp:CheckBox ID="chkIsForShiftFooter" runat="server" />
                            <asp:Label runat="server" AssociatedControlID="chkIsForShiftFooter" class="LabelForCheckbox"></asp:Label>
                        </FooterTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkIsForShiftEdit" runat="server" Checked='<%#  Convert.ToBoolean( DataBinder.Eval(Container.DataItem, "IsHolidayForShift")) %>' />
                            <asp:Label runat="server" AssociatedControlID="chkIsForShiftEdit" class="LabelForCheckbox"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Action">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbutEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false" Enabled="false"></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                        <FooterTemplate>
                            <asp:LinkButton ID="btnAdd" runat="server" Text="ADD" CommandName="btnAdd" Enabled="false" Visible="false"></asp:LinkButton>
                            <asp:LinkButton ID="Linkbutton1" runat="server" Text="Cancel" CommandName="btnCancel"
                                CausesValidation="false" Enabled="false" Visible="false"></asp:LinkButton>
                        </FooterTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkbutUpdate" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>&nbsp;

                            <asp:LinkButton ID="lnkbutCancel" runat="server" Text="Cancel" CommandName="btnCancel"
                                CausesValidation="false"></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
        </div>
    </section>
</asp:Content>